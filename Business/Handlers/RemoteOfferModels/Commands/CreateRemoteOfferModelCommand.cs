
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
using Business.Handlers.RemoteOfferModels.ValidationRules;

namespace Business.Handlers.RemoteOfferModels.Commands
{
    public class CreateRemoteOfferModelCommand : IRequest<IResult>
    {
        public string ProjectId { get; set; }
        public ProductModel[] ProductList { get; set; }
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


        public class CreateRemoteOfferModelCommandHandler : IRequestHandler<CreateRemoteOfferModelCommand, IResult>
        {
            private readonly IRemoteOfferModelRepository _remoteOfferModelRepository;
            private readonly IMediator _mediator;
            public CreateRemoteOfferModelCommandHandler(IRemoteOfferModelRepository remoteOfferModelRepository, IMediator mediator)
            {
                _remoteOfferModelRepository = remoteOfferModelRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateRemoteOfferModelValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateRemoteOfferModelCommand request, CancellationToken cancellationToken)
            {
                var isThereRemoteOfferModelRecord =
                    _remoteOfferModelRepository
                    .Any(u =>u.ProjectId == request.ProjectId &&
                    u.Name == request.Name &&
                    u.Version == request.Version);

                if (isThereRemoteOfferModelRecord)
                    return new ErrorResult(Messages.NameAlreadyExist);

                var addedRemoteOfferModel = new RemoteOfferModel
                {
                    FirstPrice = request.FirstPrice,
                    LastPrice = request.LastPrice,
                    Version = request.Version,
                    IsGift = request.IsGift,
                    IsActive = request.IsActive,
                    GiftTexture = request.GiftTexture,
                    ValidityPeriod = request.ValidityPeriod,
                    StartTime = request.StartTime,
                    FinishTime = request.FinishTime,
                    Name = request.Name,
                    PlayerPercent = request.PlayerPercent,
                    ProductList = request.ProductList,
                    ProjectId = request.ProjectId
                };

                await _remoteOfferModelRepository.AddAsync(addedRemoteOfferModel);

                return new SuccessResult(Messages.Added);
            }
        }
    }
}