using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Business.BusinessAspects;
using Business.Constants;
using Business.Handlers.AdvStrategies.Query;
using Business.Handlers.InterstielAdModels.ValidationRules;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.MessageBrokers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Dtos;
using MediatR;

namespace Business.Handlers.InterstitialAdModels.Commands
{
    public class UpdateInterstitialAdModelCommand : IRequest<IResult>
    {
        public long ProjectId { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public int PlayerPercent { get; set; }
        public bool IsAdvSettingsActive { get; set; }

        public class
            UpdateInterstitialAdModelCommandHandler : IRequestHandler<UpdateInterstitialAdModelCommand, IResult>
        {
            private readonly IInterstielAdModelRepository _interstitialAdModelRepository;
            private readonly IMediator _mediator;
            private readonly IMessageBroker _messageBroker;

            public UpdateInterstitialAdModelCommandHandler(IInterstielAdModelRepository interstitialAdModelRepository,
                IMediator mediator, IMessageBroker messageBroker)
            {
                _interstitialAdModelRepository = interstitialAdModelRepository;
                _mediator = mediator;
                _messageBroker = messageBroker;
            }

            [ValidationAspect(typeof(UpdateInterstielAdModelValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(ConsoleLogger))]
            [SecuredOperation(Priority = 1)]
            [TransactionScopeAspectAsync]
            public async Task<IResult> Handle(UpdateInterstitialAdModelCommand request,
                CancellationToken cancellationToken)
            {
                var isValid = await _interstitialAdModelRepository.AnyAsync(i =>
                    i.ProjectId == request.ProjectId && i.Name == request.Name &&
                    i.Version == request.Version && i.Status == true);
                if (!isValid) return new ErrorResult(Messages.NoContent);

                var resultData = await _interstitialAdModelRepository.GetAsync(i =>
                    i.ProjectId == request.ProjectId && i.Name == request.Name &&
                    i.Version == request.Version);

                resultData.PlayerPercent = request.PlayerPercent;
                resultData.IsAdvSettingsActive = request.IsAdvSettingsActive;

                await _interstitialAdModelRepository.UpdateAsync(resultData);
                if (request.IsAdvSettingsActive)
                {
                    var interstitialAdModelDto = new InterstitialAdModelDto
                    {
                        ProjectId = request.ProjectId,
                        Name = request.Name,
                        Version = request.Version,
                        PlayerPercent = request.PlayerPercent,
                        IsAdvSettingsActive = request.IsAdvSettingsActive
                    };
                    var resultAtvStrategies = await _mediator.Send(new GetAdvStrategyQuery
                    {
                        StrategyName = request.Name,
                        StrategyVersion = request.Version
                    }, cancellationToken);

                    if (resultAtvStrategies.Data.Any())
                        interstitialAdModelDto.AdvStrategies = resultAtvStrategies.Data.ToArray();

                    await _messageBroker.SendMessageAsync(interstitialAdModelDto);
                }

                return new SuccessResult(Messages.Updated);
            }
        }
    }
}