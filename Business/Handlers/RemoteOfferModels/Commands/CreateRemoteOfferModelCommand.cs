using Business.BusinessAspects;
using Business.Constants;
using Business.Handlers.RemoteOfferModels.ValidationRules;
using Business.Handlers.RemoteOfferProductModels.Commands;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using MediatR;

namespace Business.Handlers.RemoteOfferModels.Commands;

public class CreateRemoteOfferModelCommand : IRequest<IResult>
{
    public long ProjectId { get; set; }
    public RemoteOfferProductModelDto[] ProductDtos { get; set; }
    public string Name { get; set; }
    public float FirstPrice { get; set; }
    public float LastPrice { get; set; }
    public string Version { get; set; }
    public int PlayerPercent { get; set; }
    public bool IsGift { get; set; }
    public byte[] GiftTexture { get; set; }
    public int ValidityPeriod { get; set; }
    public long StartTime { get; set; }
    public long FinishTime { get; set; }


    public class CreateRemoteOfferModelCommandHandler : IRequestHandler<CreateRemoteOfferModelCommand, IResult>
    {
        private readonly IMediator _mediator;
        private readonly IRemoteOfferModelRepository _remoteOfferModelRepository;

        public CreateRemoteOfferModelCommandHandler(IRemoteOfferModelRepository remoteOfferModelRepository,
            IMediator mediator)
        {
            _remoteOfferModelRepository = remoteOfferModelRepository;
            _mediator = mediator;
        }

        [ValidationAspect(typeof(RemoteOfferModelValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        [LogAspect(typeof(ConsoleLogger))]
        [SecuredOperation(Priority = 1)]
        public async Task<IResult> Handle(CreateRemoteOfferModelCommand request,
            CancellationToken cancellationToken)
        {
            var isThereRemoteOfferModelRecord =
                await _remoteOfferModelRepository
                    .AnyAsync(u => u.ProjectId == request.ProjectId &&
                                   u.Name == request.Name &&
                                   u.Version == request.Version &&
                                   u.Status == true);

            if (isThereRemoteOfferModelRecord)
                return new ErrorResult(Messages.NameAlreadyExist);

            var addedRemoteOfferModel = new RemoteOfferModel
            {
                FirstPrice = request.FirstPrice,
                LastPrice = request.LastPrice,
                Version = request.Version,
                IsGift = request.IsGift,
                ValidityPeriod = request.ValidityPeriod,
                StartTime = request.StartTime,
                FinishTime = request.FinishTime,
                Name = request.Name,
                PlayerPercent = request.PlayerPercent,
                ProjectId = request.ProjectId
            };
            if (!(request.GiftTexture.Length > 0))
                addedRemoteOfferModel.GiftTexture = request.GiftTexture;

            await _remoteOfferModelRepository.AddAsync(addedRemoteOfferModel);
            foreach (var product in request.ProductDtos)
                await _mediator.Send(new CreateRemoteOfferProductModelCommand
                {
                    Count = product.Count,
                    Image = product.Image,
                    Name = product.Name,
                    ImageName = product.ImageName,
                    ProjectId = addedRemoteOfferModel.ProjectId,
                    Version = addedRemoteOfferModel.Version,
                    RemoteOfferName = addedRemoteOfferModel.Name
                }, cancellationToken);

            return new SuccessResult(Messages.Added);
        }
    }
}