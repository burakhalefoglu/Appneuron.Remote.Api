
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
using Business.Handlers.RemoteOfferModels.ValidationRules;

namespace Business.Handlers.RemoteOfferModels.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateRemoteOfferModelCommand : IRequest<IResult>
    {

        public float FirstPrice { get; set; }
        public float LastPrice { get; set; }
        public int OfferId { get; set; }
        public bool IsGift { get; set; }
        public bool IsActive { get; set; }
        public byte[] GiftTexture { get; set; }
        public int ValidityPeriod { get; set; }
        public System.DateTime StartTime { get; set; }
        public System.DateTime FinishTime { get; set; }


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
                var isThereRemoteOfferModelRecord = _remoteOfferModelRepository.Any(u => u.FirstPrice == request.FirstPrice);

                if (isThereRemoteOfferModelRecord == true)
                    return new ErrorResult(Messages.NameAlreadyExist);

                var addedRemoteOfferModel = new RemoteOfferModel
                {
                    FirstPrice = request.FirstPrice,
                    LastPrice = request.LastPrice,
                    OfferId = request.OfferId,
                    IsGift = request.IsGift,
                    IsActive = request.IsActive,
                    GiftTexture = request.GiftTexture,
                    ValidityPeriod = request.ValidityPeriod,
                    StartTime = request.StartTime,
                    FinishTime = request.FinishTime,

                };

                await _remoteOfferModelRepository.AddAsync(addedRemoteOfferModel);

                return new SuccessResult(Messages.Added);
            }
        }
    }
}