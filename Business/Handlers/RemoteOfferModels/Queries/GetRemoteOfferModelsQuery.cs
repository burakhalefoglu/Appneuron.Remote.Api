﻿
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

namespace Business.Handlers.RemoteOfferModels.Queries
{

    public class GetRemoteOfferModelsQuery : IRequest<IDataResult<IEnumerable<RemoteOfferModel>>>
    {
        public class GetRemoteOfferModelsQueryHandler : IRequestHandler<GetRemoteOfferModelsQuery, IDataResult<IEnumerable<RemoteOfferModel>>>
        {
            private readonly IRemoteOfferModelRepository _remoteOfferModelRepository;
            private readonly IMediator _mediator;

            public GetRemoteOfferModelsQueryHandler(IRemoteOfferModelRepository remoteOfferModelRepository, IMediator mediator)
            {
                _remoteOfferModelRepository = remoteOfferModelRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<RemoteOfferModel>>> Handle(GetRemoteOfferModelsQuery request, CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<RemoteOfferModel>>(await _remoteOfferModelRepository.GetListAsync());
            }
        }
    }
}