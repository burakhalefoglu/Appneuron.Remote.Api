
using Business.BusinessAspects;
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Business.Handlers.RemoteOfferHistoryModels.ValidationRules;
using ServiceStack;

namespace Business.Handlers.RemoteOfferHistoryModels.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateRemoteOfferHistoryModelCommand : IRequest<IResult>
    {

        public string ProjectId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public float FirstPrice { get; set; }
        public float LastPrice { get; set; }
        public int Version { get; set; }
        public int PlayerPercent { get; set; }
        public bool IsGift { get; set; }
        public byte[] GiftTexture { get; set; }
        public int ValidityPeriod { get; set; }
        public long StartTime { get; set; }
        public long FinishTime { get; set; }

        public class CreateRemoteOfferHistoryModelCommandHandler : IRequestHandler<CreateRemoteOfferHistoryModelCommand, IResult>
        {
            private readonly IRemoteOfferHistoryModelRepository _remoteOfferHistoryModelRepository;
            private readonly IMediator _mediator;
            public CreateRemoteOfferHistoryModelCommandHandler(IRemoteOfferHistoryModelRepository remoteOfferHistoryModelRepository, IMediator mediator)
            {
                _remoteOfferHistoryModelRepository = remoteOfferHistoryModelRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateRemoteOfferHistoryModelValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateRemoteOfferHistoryModelCommand request, CancellationToken cancellationToken)
            {
                var addedRemoteOfferHistoryModel = new RemoteOfferHistoryModel
                {
                    ProjectId = request.ProjectId,
                    Name = request.Name,
                    IsActive = request.IsActive,
                    FirstPrice = request.FirstPrice,
                    LastPrice = request.LastPrice,
                    Version = request.Version,
                    PlayerPercent = request.PlayerPercent,
                    IsGift = request.IsGift,
                    GiftTexture = request.GiftTexture,
                    ValidityPeriod = request.ValidityPeriod,
                    StartTime = request.StartTime,
                    FinishTime = request.FinishTime,

                };

                await _remoteOfferHistoryModelRepository.AddAsync(addedRemoteOfferHistoryModel);

                return new SuccessResult(Messages.Added);
            }
        }
    }
}