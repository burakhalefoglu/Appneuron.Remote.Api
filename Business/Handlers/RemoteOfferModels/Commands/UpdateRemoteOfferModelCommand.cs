using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Business.BusinessAspects;
using Business.Constants;
using Business.Handlers.RemoteOfferModels.ValidationRules;
using Business.Handlers.RemoteOfferProductModels.Queries;
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

namespace Business.Handlers.RemoteOfferModels.Commands
{
    public class UpdateRemoteOfferModelCommand : IRequest<IResult>
    {
        public long ProjectId { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public int PlayerPercent { get; set; }
        public bool IsActive { get; set; }

        public class UpdateRemoteOfferModelCommandHandler : IRequestHandler<UpdateRemoteOfferModelCommand, IResult>
        {
            private readonly IMediator _mediator;
            private readonly IRemoteOfferModelRepository _remoteOfferModelRepository;
            private readonly IMessageBroker _messageBroker;

            public UpdateRemoteOfferModelCommandHandler(IRemoteOfferModelRepository remoteOfferModelRepository,
                IMediator mediator, IMessageBroker messageBroker)
            {
                _remoteOfferModelRepository = remoteOfferModelRepository;
                _mediator = mediator;
                _messageBroker = messageBroker;
            }

            [ValidationAspect(typeof(UpdateRemoteOfferModelValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(ConsoleLogger))]
            [TransactionScopeAspectAsync]
            [SecuredOperation(Priority = 1)]

            public async Task<IResult> Handle(UpdateRemoteOfferModelCommand request,
                CancellationToken cancellationToken)
            {
                var isValid = await _remoteOfferModelRepository.AnyAsync(r => r.ProjectId == request.ProjectId &&
                                                                   r.Name == request.Name &&
                                                                   r.Version == request.Version &&
                                                                   r.Status == true);
                if (!isValid) return new ErrorResult(Messages.NoContent);

                var resultData = await 
                    _remoteOfferModelRepository.GetAsync(r =>
                    r.ProjectId == request.ProjectId &&
                    r.Name == request.Name &&
                    r.Version == request.Version);

                resultData.PlayerPercent = request.PlayerPercent;
                resultData.IsActive = request.IsActive;
                if (request.IsActive)
                {
                    resultData.StartTime = DateTime.Now.Ticks;
                    resultData.FinishTime = DateTime.Now.AddHours(resultData.ValidityPeriod).Ticks;
                    var remoteOfferModelDto = new RemoteOfferModelDto
                    {
                        Name = request.Name,
                        Version = request.Version,
                        FinishTime = resultData.FinishTime,
                        FirstPrice = resultData.FirstPrice,
                        GiftTexture = resultData.GiftTexture,
                        IsActive = request.IsActive,
                        IsGift = resultData.IsGift,
                        LastPrice = resultData.LastPrice,
                        PlayerPercent = request.PlayerPercent,
                        ProjectId = request.ProjectId,
                        StartTime = resultData.StartTime,
                        ValidityPeriod = resultData.ValidityPeriod,
                    };
                    var resultProductModels = await _mediator.Send(new GetRemoteOfferProductModelsQuery
                    {
                        Version = resultData.Version,
                        RemoteOfferName = resultData.Name
                    }, cancellationToken);
                    
                    if (resultProductModels.Data.Any())
                        remoteOfferModelDto.RemoteOfferProductModels = resultProductModels.Data.ToArray();
                  
                    await _messageBroker.SendMessageAsync(remoteOfferModelDto);
                }
                await _remoteOfferModelRepository.UpdateAsync(resultData);

                return new SuccessResult(Messages.Updated);
            }
        }
    }
}