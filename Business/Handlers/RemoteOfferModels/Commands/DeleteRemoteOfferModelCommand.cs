using Business.BusinessAspects;
using Business.Constants;
using Business.Handlers.RemoteOfferProductModels.Commands;
using Business.Handlers.RemoteOfferProductModels.Queries;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using MediatR;

namespace Business.Handlers.RemoteOfferModels.Commands;

/// <summary>
/// </summary>
public class DeleteRemoteOfferModelCommand : IRequest<IResult>
{
    public long ProjectId { get; set; }
    public string Name { get; set; }
    public string Version { get; set; }

    public class DeleteRemoteOfferModelCommandHandler : IRequestHandler<DeleteRemoteOfferModelCommand, IResult>
    {
        private readonly IMediator _mediator;
        private readonly IRemoteOfferModelRepository _remoteOfferModelRepository;

        public DeleteRemoteOfferModelCommandHandler(IRemoteOfferModelRepository remoteOfferModelRepository,
            IMediator mediator)
        {
            _remoteOfferModelRepository = remoteOfferModelRepository;
            _mediator = mediator;
        }

        [CacheRemoveAspect("Get")]
        [LogAspect(typeof(ConsoleLogger))]
        [SecuredOperation(Priority = 1)]
        public async Task<IResult> Handle(DeleteRemoteOfferModelCommand request,
            CancellationToken cancellationToken)
        {
            var isThereInterstitialAdModelRecord = await _remoteOfferModelRepository.GetAsync(u =>
                u.Name == request.Name &&
                u.ProjectId == request.ProjectId &&
                u.Version == request.Version &&
                u.Status == true);

            if (isThereInterstitialAdModelRecord is null)
                return new ErrorResult(Messages.NotFound);
            isThereInterstitialAdModelRecord.IsActive = false;

            await _remoteOfferModelRepository.DeleteAsync(isThereInterstitialAdModelRecord);
            var products = (await _mediator.Send(new GetRemoteOfferProductModelsQuery
            {
              StrategyId = isThereInterstitialAdModelRecord.Id
            })).Data.ToList();
            foreach (var product in products)
                await _mediator.Send(new DeleteRemoteOfferProductModelCommand
                {
                 Id = product.Id,
                 StrategyId = product.StrategyId
                }, cancellationToken);

            return new SuccessResult(Messages.Deleted);
        }
    }
}