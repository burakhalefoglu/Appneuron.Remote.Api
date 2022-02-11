using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Business.BusinessAspects;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Performance;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;

namespace Business.Handlers.RemoteOfferHistoryModels.Queries
{
    public class GetOfferHistoryModelsByProjectIdQuery : IRequest<IDataResult<IEnumerable<RemoteOfferHistoryModel>>>
    {
        public long ProjectId { get; set; }

        public class GetOfferHistoryModelsByProjectIdQueryHandler : IRequestHandler<
            GetOfferHistoryModelsByProjectIdQuery, IDataResult<IEnumerable<RemoteOfferHistoryModel>>>
        {
            private readonly IMediator _mediator;
            private readonly IRemoteOfferHistoryModelRepository _remoteOfferHistoryModelRepository;

            public GetOfferHistoryModelsByProjectIdQueryHandler(
                IRemoteOfferHistoryModelRepository remoteOfferHistoryModelRepository, IMediator mediator)
            {
                _remoteOfferHistoryModelRepository = remoteOfferHistoryModelRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(ConsoleLogger))]
public async Task<IDataResult<IEnumerable<RemoteOfferHistoryModel>>> Handle(
                GetOfferHistoryModelsByProjectIdQuery request, CancellationToken cancellationToken)
            {
                var result = await _remoteOfferHistoryModelRepository
                    .GetListAsync(r => r.ProjectId == request.ProjectId);

                return new SuccessDataResult<IEnumerable<RemoteOfferHistoryModel>>(result);
            }
        }
    }
}