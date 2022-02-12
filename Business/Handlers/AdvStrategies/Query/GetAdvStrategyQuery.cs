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

namespace Business.Handlers.AdvStrategies.Query
{
    public class GetAdvStrategyQuery : IRequest<IDataResult<IEnumerable<AdvStrategy>>>
    {
        public string StrategyName { get; set; }
        public string StrategyVersion { get; set; }
        
        public class GetAdvStrategyQueryHandler : IRequestHandler<
            GetAdvStrategyQuery,
            IDataResult<IEnumerable<AdvStrategy>>>
        {
            private readonly IAdvStrategyRepository _advStrategyRepository;

            public GetAdvStrategyQueryHandler(IAdvStrategyRepository advStrategyRepository)
            {
                _advStrategyRepository = advStrategyRepository;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(ConsoleLogger))]
            [SecuredOperation(Priority = 1)]

            public async Task<IDataResult<IEnumerable<AdvStrategy>>> Handle(
                GetAdvStrategyQuery request, CancellationToken cancellationToken)
            {
                var result = await _advStrategyRepository
                    .GetListAsync(r => r.StrategyName == request.StrategyName && r.StrategyVersion == request.StrategyVersion && r.Status == true);
                
                return new SuccessDataResult<IEnumerable<AdvStrategy>>(result);
            }
        }
    }
}