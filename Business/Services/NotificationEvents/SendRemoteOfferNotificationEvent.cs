using Business.BusinessAspects;
using Business.Handlers.RemoteOfferProductModels.Queries;
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

public class SendRemoteOfferNotificationEvent : IRequest<IResult>
{
    public long ProjectId { get; set; }
    
    public class SendRemoteOfferNotificationEventHandler : IRequestHandler<
        SendRemoteOfferNotificationEvent,
        IResult>
    {
        private readonly IMediator _mediator;
        private readonly IRemoteOfferModelRepository _remoteOfferModelRepository;
        private readonly IMessageBroker _messageBroker;

        public SendRemoteOfferNotificationEventHandler(
            IRemoteOfferModelRepository remoteOfferModelRepository, IMediator mediator)
        {
            _remoteOfferModelRepository = remoteOfferModelRepository;
            _mediator = mediator;
        }

        [PerformanceAspect(5)]
        [CacheAspect(10)]
        [LogAspect(typeof(ConsoleLogger))]
        [SecuredOperation(Priority = 1)]
        public async Task<IResult> Handle(
            SendRemoteOfferNotificationEvent request, CancellationToken cancellationToken)
        {
            var result = _remoteOfferModelRepository
                .GetListAsync().Result.Where(r => r.ProjectId == request.ProjectId &&
                                                  r.Status == true &&
                                                  r.IsActive);
            var remoteOfferModelDtos = new List<RemoteOfferClientModelDto>();
            foreach (var remoteOfferModel in result)
            {
                var resultProductModels = await _mediator.Send(new GetRemoteOfferProductModelsQuery
                {
                 StrategyId = remoteOfferModel.Id
                }, cancellationToken);

                var remoteOfferModelDto = new RemoteOfferClientModelDto
                {
                    FinishTime = remoteOfferModel.FinishTime,
                    FirstPrice = remoteOfferModel.FirstPrice,
                    GiftTexture = remoteOfferModel.GiftTexture,
                    IsGift = remoteOfferModel.IsGift,
                    LastPrice = remoteOfferModel.LastPrice,
                    PlayerPercent = remoteOfferModel.PlayerPercent,
                    ProjectId = remoteOfferModel.ProjectId,
                    StartTime = remoteOfferModel.StartTime,
                    ValidityPeriod = remoteOfferModel.ValidityPeriod,
                };
                foreach (var remoteOfferProductModel in resultProductModels.Data)
                {
                    var remoteOfferProductDto = new RemoteOfferProductClientModelDto();
                    remoteOfferProductDto.Count = remoteOfferProductModel.Count;
                    remoteOfferProductDto.Image =  remoteOfferProductModel.Image;
                    remoteOfferProductDto.Name = remoteOfferProductModel.Name;
                    remoteOfferModelDto.RemoteOfferProductClientModelDtos.Add(remoteOfferProductDto);
                }
                remoteOfferModelDtos.Add(remoteOfferModelDto);
            }
            await _messageBroker.SendMessageAsync(remoteOfferModelDtos);
            return new SuccessResult();
        }
    }
}