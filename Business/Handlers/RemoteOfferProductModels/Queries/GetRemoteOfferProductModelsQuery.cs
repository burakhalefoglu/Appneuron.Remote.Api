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
    public string RemoteOfferName { get; set; }
    public string Version { get; set; }
    public long ProjectId { get; set; }

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
            var result = await _remoteOfferProductModelRepository
                .GetListAsync(r => r.RemoteOfferName == request.RemoteOfferName &&
                                   r.Version == request.Version &&
                                   r.ProjectId == request.ProjectId &&
                                   r.Status == true);

            return new SuccessDataResult<IEnumerable<RemoteOfferProductModel>>(result);
        }
    }
}