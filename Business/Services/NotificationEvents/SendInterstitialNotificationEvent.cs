using Business.BusinessAspects;
using Business.Handlers.AdvStrategies.Query;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Performance;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.MessageBrokers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Dtos;
using MediatR;

namespace Business.Services.NotificationEvents;

public class SendInterstitialNotificationEvent : IRequest<IResult>
{
    public long ProjectId { get; set; }

    public class SendInterstitialNotificationEventHandler : IRequestHandler<
        SendInterstitialNotificationEvent, IResult>
    {
        private readonly IInterstielAdModelRepository _interstitialAdModelRepository;
        private readonly IMediator _mediator;
        private readonly IMessageBroker _messageBroker;

        public SendInterstitialNotificationEventHandler(
            IInterstielAdModelRepository interstitialAdModelRepository, IMediator mediator,
            IMessageBroker messageBroker)
        {
            _interstitialAdModelRepository = interstitialAdModelRepository;
            _mediator = mediator;
            _messageBroker = messageBroker;
        }

        [PerformanceAspect(5)]
        [CacheAspect(10)]
        [LogAspect(typeof(ConsoleLogger))]
        [SecuredOperation(Priority = 1)]
        public async Task<IResult> Handle(
            SendInterstitialNotificationEvent request, CancellationToken cancellationToken)
        {
            var interstitialAdModelDtos = new List<InterstitialAdClientModelDto>();
            var result = _interstitialAdModelRepository
                .GetListAsync().Result.Where(u =>
                    u.ProjectId == request.ProjectId &&
                    u.Status == true &&
                    u.IsActive);

            foreach (var ınterstitialAdModel in result)
            {
                var resultAdvStrategies = await _mediator.Send(new GetAdvStrategyQuery
                {
                    StrategyId = ınterstitialAdModel.Id
                }, cancellationToken);
                var interstitialAdModelDto = new InterstitialAdClientModelDto
                {
                    PlayerPercent = ınterstitialAdModel.PlayerPercent,
                    ProjectId = ınterstitialAdModel.ProjectId,
                };
                foreach (var advStrategy in resultAdvStrategies.Data)
                {
                    var advStrategyClientDto = new AdvStrategyClientDto
                    {
                        Name = advStrategy.Name,
                        StrategyValue = advStrategy.StrategyValue
                    };
                    interstitialAdModelDto.AdvStrategyClientDto.Add(advStrategyClientDto);
                }

                interstitialAdModelDtos.Add(interstitialAdModelDto);
            }

            await _messageBroker.SendMessageAsync(interstitialAdModelDtos);
            return new SuccessResult();
        }
    }
}