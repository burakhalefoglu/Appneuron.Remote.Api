using System;
using System.Threading;
using System.Threading.Tasks;
using Business.BusinessAspects;
using Business.Constants;
using Business.Handlers.RemoteOfferHistoryModels.Commands;
using Business.Handlers.RemoteOfferModels.ValidationRules;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using MediatR;

namespace Business.Handlers.RemoteOfferModels.Commands
{
    public class UpdateRemoteOfferModelCommand : IRequest<IResult>
    {
        public string ProjectId { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public int PlayerPercent { get; set; }
        public bool IsActive { get; set; }

        public class UpdateRemoteOfferModelCommandHandler : IRequestHandler<UpdateRemoteOfferModelCommand, IResult>
        {
            private readonly IMediator _mediator;
            private readonly IRemoteOfferModelRepository _remoteOfferModelRepository;

            public UpdateRemoteOfferModelCommandHandler(IRemoteOfferModelRepository remoteOfferModelRepository,
                IMediator mediator)
            {
                _remoteOfferModelRepository = remoteOfferModelRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdateRemoteOfferModelValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
<<<<<<< Updated upstream
            [LogAspect(typeof(ConsoleLogger))]
=======
            [LogAspect(typeof(LogstashLogger))]
>>>>>>> Stashed changes
            [SecuredOperation(Priority = 1)]
            [TransactionScopeAspectAsync]
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
                }

                await _mediator.Send(new CreateRemoteOfferHistoryModelCommand
                {
                    ProjectId = resultData.ProjectId,
                    Name = resultData.Name,
                    IsActive = request.IsActive,
                    FirstPrice = resultData.FirstPrice,
                    LastPrice = resultData.LastPrice,
                    Version = resultData.Version,
                    PlayerPercent = resultData.PlayerPercent,
                    IsGift = resultData.IsGift,
                    GiftTexture = resultData.GiftTexture,
                    ValidityPeriod = resultData.ValidityPeriod,
                    StartTime = resultData.StartTime,
                    FinishTime = resultData.FinishTime
                });

                await _remoteOfferModelRepository.UpdateAsync(resultData,
                    i => i.ProjectId == request.ProjectId && i.Name == request.Name &&
                         i.Version == request.Version);

                return new SuccessResult(Messages.Updated);
            }
        }
    }
}