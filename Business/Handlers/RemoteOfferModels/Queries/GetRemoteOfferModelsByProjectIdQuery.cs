
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

namespace Business.Handlers.RemoteOfferModels.Queries
{

    public class GetRemoteOfferModelsByProjectIdQuery : IRequest<IDataResult<IEnumerable<RemoteOfferModel>>>
    {
        public string ProjectId { get; set; }

        public class GetRemoteOfferModelsByProjectIdQueryHandler : IRequestHandler<GetRemoteOfferModelsByProjectIdQuery, IDataResult<IEnumerable<RemoteOfferModel>>>
        {
            private readonly IRemoteOfferModelRepository _remoteOfferModelRepository;
            private readonly IMediator _mediator;

            public GetRemoteOfferModelsByProjectIdQueryHandler(IRemoteOfferModelRepository remoteOfferModelRepository, IMediator mediator)
            {
                _remoteOfferModelRepository = remoteOfferModelRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<RemoteOfferModel>>> Handle(GetRemoteOfferModelsByProjectIdQuery request, CancellationToken cancellationToken)
            {
                var result = await _remoteOfferModelRepository
                    .GetListAsync(r => r.ProjectId == request.ProjectId);

                return new SuccessDataResult<IEnumerable<RemoteOfferModel>>(result);
            }
        }
    }
}