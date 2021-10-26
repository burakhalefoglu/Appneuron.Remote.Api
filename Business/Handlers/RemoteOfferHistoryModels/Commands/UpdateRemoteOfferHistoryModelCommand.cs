
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
using Business.Handlers.RemoteOfferHistoryModels.ValidationRules;
using MongoDB.Bson;

namespace Business.Handlers.RemoteOfferHistoryModels.Commands
{


    public class UpdateRemoteOfferHistoryModelCommand : IRequest<IResult>
    {
        public string ObjectId { get; set; }
        private ObjectId Id => new ObjectId(this.ObjectId);
        public string ProjectId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public float FirstPrice { get; set; }
        public float LastPrice { get; set; }
        public int Version { get; set; }
        public int PlayerPercent { get; set; }
        public bool IsGift { get; set; }
        public string GiftTexture { get; set; }
        public int ValidityPeriod { get; set; }
        public long StartTime { get; set; }
        public long FinishTime { get; set; }

        public class UpdateRemoteOfferHistoryModelCommandHandler : IRequestHandler<UpdateRemoteOfferHistoryModelCommand, IResult>
        {
            private readonly IRemoteOfferHistoryModelRepository _remoteOfferHistoryModelRepository;
            private readonly IMediator _mediator;

            public UpdateRemoteOfferHistoryModelCommandHandler(IRemoteOfferHistoryModelRepository remoteOfferHistoryModelRepository, IMediator mediator)
            {
                _remoteOfferHistoryModelRepository = remoteOfferHistoryModelRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdateRemoteOfferHistoryModelValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdateRemoteOfferHistoryModelCommand request, CancellationToken cancellationToken)
            {



                var remoteOfferHistoryModel = new RemoteOfferHistoryModel();
                remoteOfferHistoryModel.ProjectId = request.ProjectId;
                remoteOfferHistoryModel.Name = request.Name;
                remoteOfferHistoryModel.IsActive = request.IsActive;
                remoteOfferHistoryModel.FirstPrice = request.FirstPrice;
                remoteOfferHistoryModel.LastPrice = request.LastPrice;
                remoteOfferHistoryModel.Version = request.Version;
                remoteOfferHistoryModel.PlayerPercent = request.PlayerPercent;
                remoteOfferHistoryModel.IsGift = request.IsGift;
                remoteOfferHistoryModel.GiftTexture = request.GiftTexture;
                remoteOfferHistoryModel.ValidityPeriod = request.ValidityPeriod;
                remoteOfferHistoryModel.StartTime = request.StartTime;
                remoteOfferHistoryModel.FinishTime = request.FinishTime;


                await _remoteOfferHistoryModelRepository.UpdateAsync(request.Id, remoteOfferHistoryModel);

                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

