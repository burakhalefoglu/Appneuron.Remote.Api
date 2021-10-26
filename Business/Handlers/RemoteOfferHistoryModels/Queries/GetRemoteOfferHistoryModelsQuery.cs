
using Business.BusinessAspects;
using Core.Aspects.Autofac.Performance;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Aspects.Autofac.Caching;

namespace Business.Handlers.RemoteOfferHistoryModels.Queries
{

    public class GetRemoteOfferHistoryModelsQuery : IRequest<IDataResult<IEnumerable<RemoteOfferHistoryModel>>>
    {
        public class GetRemoteOfferHistoryModelsQueryHandler : IRequestHandler<GetRemoteOfferHistoryModelsQuery, IDataResult<IEnumerable<RemoteOfferHistoryModel>>>
        {
            private readonly IRemoteOfferHistoryModelRepository _remoteOfferHistoryModelRepository;
            private readonly IMediator _mediator;

            public GetRemoteOfferHistoryModelsQueryHandler(IRemoteOfferHistoryModelRepository remoteOfferHistoryModelRepository, IMediator mediator)
            {
                _remoteOfferHistoryModelRepository = remoteOfferHistoryModelRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<RemoteOfferHistoryModel>>> Handle(GetRemoteOfferHistoryModelsQuery request, CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<RemoteOfferHistoryModel>>(await _remoteOfferHistoryModelRepository.GetListAsync());
            }
        }
    }
}