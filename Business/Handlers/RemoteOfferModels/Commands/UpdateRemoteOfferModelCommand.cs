
using Business.Constants;
using Business.BusinessAspects;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Core.Aspects.Autofac.Validation;
using Business.Handlers.RemoteOfferModels.ValidationRules;
using MongoDB.Bson;
using Business.Handlers.RemoteOfferHistoryModels.Commands;
using System;

namespace Business.Handlers.RemoteOfferModels.Commands
{


    public class UpdateRemoteOfferModelCommand : IRequest<IResult>
    {
        public string ProjectId { get; set; }
        public string Name { get; set; }
        public float Version { get; set; }
        public int playerPercent { get; set; }
        public bool IsActive { get; set; }

        public class UpdateRemoteOfferModelCommandHandler : IRequestHandler<UpdateRemoteOfferModelCommand, IResult>
        {
            private readonly IRemoteOfferModelRepository _remoteOfferModelRepository;
            private readonly IMediator _mediator;

            public UpdateRemoteOfferModelCommandHandler(IRemoteOfferModelRepository remoteOfferModelRepository, IMediator mediator)
            {
                _remoteOfferModelRepository = remoteOfferModelRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdateRemoteOfferModelValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdateRemoteOfferModelCommand request, CancellationToken cancellationToken)
            {

                var resultData = await _remoteOfferModelRepository.GetByFilterAsync(r => r.ProjectId == request.ProjectId &&
                r.Name == request.Name &&
                r.Version == request.Version);
                if (resultData == null)
                {
                    return new ErrorResult(Messages.NoContent);
                }
                resultData.PlayerPercent = request.playerPercent;
                resultData.IsActive = request.IsActive;
                if (request.IsActive)
                {
                    resultData.StartTime = DateTime.Now.Ticks;
                    resultData.FinishTime = DateTime.Now.AddHours(resultData.ValidityPeriod).Ticks;
                }
                await _remoteOfferModelRepository.UpdateAsync(resultData,
                    i => i.ProjectId == request.ProjectId && i.Name == request.Name &&
                    i.Version == request.Version);

                    await _mediator.Send(new CreateRemoteOfferHistoryModelCommand
                    {
                        ProjectId = resultData.ProjectId,
                        ProductList = resultData.ProductList,
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
                

                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

