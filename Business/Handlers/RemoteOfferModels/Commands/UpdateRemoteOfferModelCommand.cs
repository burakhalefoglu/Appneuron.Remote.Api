
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

namespace Business.Handlers.RemoteOfferModels.Commands
{


    public class UpdateRemoteOfferModelCommand : IRequest<IResult>
    {
        public string ObjectId { get; set; }
        private ObjectId Id => new ObjectId(this.ObjectId);
        public float FirstPrice { get; set; }
        public float LastPrice { get; set; }
        public int Version { get; set; }
        public bool IsGift { get; set; }
        public bool IsActive { get; set; }
        public string GiftTexture { get; set; }
        public int ValidityPeriod { get; set; }
        public int StartTime { get; set; }
        public int FinishTime { get; set; }

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



                var remoteOfferModel = new RemoteOfferModel();
                remoteOfferModel.FirstPrice = request.FirstPrice;
                remoteOfferModel.LastPrice = request.LastPrice;
                remoteOfferModel.Version = request.Version;
                remoteOfferModel.IsGift = request.IsGift;
                remoteOfferModel.IsActive = request.IsActive;
                remoteOfferModel.GiftTexture = request.GiftTexture;
                remoteOfferModel.ValidityPeriod = request.ValidityPeriod;
                remoteOfferModel.StartTime = request.StartTime;
                remoteOfferModel.FinishTime = request.FinishTime;


                await _remoteOfferModelRepository.UpdateAsync(request.Id, remoteOfferModel);

                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

