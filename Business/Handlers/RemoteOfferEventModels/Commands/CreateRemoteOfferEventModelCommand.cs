using System.Threading;
using System.Threading.Tasks;
using Business.BusinessAspects;
using Business.Constants;
using Business.Handlers.RemoteOfferEventModels.ValidationRules;
using Business.MessageBrokers;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;

namespace Business.Handlers.RemoteOfferEventModels.Commands
{
    /// <summary>
    /// </summary>
    public class CreateRemoteOfferEventModelCommand : IRequest<IResult>
    {
        public string ProjectId { get; set; }
        public string[] ClientIdList { get; set; }
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
        public ProductModel[] ProductList { get; set; }

        public class
            CreateRemoteOfferEventModelCommandHandler : IRequestHandler<CreateRemoteOfferEventModelCommand, IResult>
        {
            private readonly IMediator _mediator;
            private readonly IMessageBroker _messageBroker;
            private readonly IRemoteOfferEventModelRepository _remoteOfferEventModelRepository;

            public CreateRemoteOfferEventModelCommandHandler(
                IRemoteOfferEventModelRepository remoteOfferEventModelRepository,
                IMediator mediator,
                IMessageBroker messageBroker)
            {
                _remoteOfferEventModelRepository = remoteOfferEventModelRepository;
                _mediator = mediator;
                _messageBroker = messageBroker;
            }

            [ValidationAspect(typeof(CreateRemoteOfferEventModelValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(ConsoleLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateRemoteOfferEventModelCommand request,
                CancellationToken cancellationToken)
            {
                var addedRemoteOfferEventModel = new RemoteOfferEventModel
                {
                    ProjectId = request.ProjectId,
                    ClientIdList = request.ClientIdList,
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
                    ProductList = request.ProductList
                };

                await _remoteOfferEventModelRepository.AddAsync(addedRemoteOfferEventModel);
                await _messageBroker.SendMessageAsync(addedRemoteOfferEventModel);

                return new SuccessResult(Messages.Added);
            }
        }
    }
}