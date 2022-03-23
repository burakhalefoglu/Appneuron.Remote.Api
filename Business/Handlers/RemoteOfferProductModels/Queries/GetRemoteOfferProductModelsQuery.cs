using Business.BusinessAspects;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Performance;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;

namespace Business.Handlers.RemoteOfferProductModels.Queries;

public class GetRemoteOfferProductModelsQuery : IRequest<IDataResult<IEnumerable<RemoteOfferProductModel>>>
{
    public long StrategyId { get; set; }

    public class GetRemoteOfferProductModelsQueryHandler : IRequestHandler<
        GetRemoteOfferProductModelsQuery,
        IDataResult<IEnumerable<RemoteOfferProductModel>>>
    {
        private readonly IRemoteOfferProductModelRepository _remoteOfferProductModelRepository;

        public GetRemoteOfferProductModelsQueryHandler(
            IRemoteOfferProductModelRepository remoteOfferProductModelRepository)
        {
            _remoteOfferProductModelRepository = remoteOfferProductModelRepository;
        }

        [PerformanceAspect(5)]
        [CacheAspect(10)]
        [LogAspect(typeof(ConsoleLogger))]
        [SecuredOperation(Priority = 1)]
        public async Task<IDataResult<IEnumerable<RemoteOfferProductModel>>> Handle(
            GetRemoteOfferProductModelsQuery request, CancellationToken cancellationToken)
        {
            var result = _remoteOfferProductModelRepository
                .GetListAsync().Result.Where(r => r.StrategyId == request.StrategyId &&
                                                  r.Status == true);

            return new SuccessDataResult<IEnumerable<RemoteOfferProductModel>>(result);
        }
    }
}