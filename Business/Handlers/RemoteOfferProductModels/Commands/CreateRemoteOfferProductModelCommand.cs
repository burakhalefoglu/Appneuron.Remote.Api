using Business.BusinessAspects;
using Business.Constants;
using Business.Handlers.RemoteOfferProductModels.ValidationRules;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;

namespace Business.Handlers.RemoteOfferProductModels.Commands;

public class CreateRemoteOfferProductModelCommand : IRequest<IResult>
{
    public string Name { get; set; }
    public byte[] Image { get; set; }
    public float Count { get; set; }
    public string ImageName { get; set; }
    public long StrategyId { get; set; }

    public class
        CreateRemoteOfferProductModelCommandHandler : IRequestHandler<CreateRemoteOfferProductModelCommand, IResult>
    {
        private readonly IRemoteOfferProductModelRepository _remoteOfferProductModelRepository;

        public CreateRemoteOfferProductModelCommandHandler(
            IRemoteOfferProductModelRepository remoteOfferProductModelRepository)
        {
            _remoteOfferProductModelRepository = remoteOfferProductModelRepository;
        }

        [ValidationAspect(typeof(RemoteOfferProductModelValidator), Priority = 2)]
        [CacheRemoveAspect("Get")]
        [LogAspect(typeof(ConsoleLogger))]
        [SecuredOperation(Priority = 1)]
        public async Task<IResult> Handle(CreateRemoteOfferProductModelCommand request,
            CancellationToken cancellationToken)
        {
            await _remoteOfferProductModelRepository.AddAsync(new RemoteOfferProductModel
            {
                Count = request.Count,
                Image = request.Image,
                Name = request.Name,
                ImageName = request.ImageName,
                StrategyId = request.StrategyId
            });

            return new SuccessResult(Messages.Added);
        }
    }
}