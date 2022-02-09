﻿using System.Collections.Generic;
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

namespace Business.Handlers.RemoteOfferModels.Queries
{
    public class GetRemoteOfferModelsByProjectIdQuery : IRequest<IDataResult<IEnumerable<RemoteOfferModel>>>
    {
        public string ProjectId { get; set; }

        public class GetRemoteOfferModelsByProjectIdQueryHandler : IRequestHandler<GetRemoteOfferModelsByProjectIdQuery,
            IDataResult<IEnumerable<RemoteOfferModel>>>
        {
            private readonly IRemoteOfferModelRepository _remoteOfferModelRepository;

            public GetRemoteOfferModelsByProjectIdQueryHandler(IRemoteOfferModelRepository remoteOfferModelRepository)
            {
                _remoteOfferModelRepository = remoteOfferModelRepository;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
<<<<<<< Updated upstream
            [LogAspect(typeof(ConsoleLogger))]
=======
            [LogAspect(typeof(LogstashLogger))]
>>>>>>> Stashed changes
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<RemoteOfferModel>>> Handle(
                GetRemoteOfferModelsByProjectIdQuery request, CancellationToken cancellationToken)
            {
                var result = await _remoteOfferModelRepository
                    .GetListAsync(r => r.ProjectId == request.ProjectId && r.Status == true);

                return new SuccessDataResult<IEnumerable<RemoteOfferModel>>(result);
            }
        }
    }
}