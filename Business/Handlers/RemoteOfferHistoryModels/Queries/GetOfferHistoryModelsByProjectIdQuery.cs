
using Business.BusinessAspects;
using Core.Utilities.Results;
using Core.Aspects.Autofac.Performance;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Caching;

namespace Business.Handlers.RemoteOfferHistoryModels.Queries
{

    public class GetOfferHistoryModelsByProjectIdQuery : IRequest<IDataResult<IEnumerable<RemoteOfferHistoryModel>>>
    {
        public string ProjectId { get; set; }

        public class GetOfferHistoryModelsByProjectIdQueryHandler : IRequestHandler<GetOfferHistoryModelsByProjectIdQuery, IDataResult<IEnumerable<RemoteOfferHistoryModel>>>
        {
            private readonly IRemoteOfferHistoryModelRepository _remoteOfferHistoryModelRepository;
            private readonly IMediator _mediator;

            public GetOfferHistoryModelsByProjectIdQueryHandler(IRemoteOfferHistoryModelRepository remoteOfferHistoryModelRepository, IMediator mediator)
            {
                _remoteOfferHistoryModelRepository = remoteOfferHistoryModelRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<RemoteOfferHistoryModel>>> Handle(GetOfferHistoryModelsByProjectIdQuery request, CancellationToken cancellationToken)
            {
                var result = await _remoteOfferHistoryModelRepository
                   .GetListAsync(r => r.ProjectId == request.ProjectId);

                return new SuccessDataResult<IEnumerable<RemoteOfferHistoryModel>>(result);
            }
        }
    }
}