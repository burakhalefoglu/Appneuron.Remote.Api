
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

namespace Business.Handlers.InterstielAdHistoryModels.Queries
{

    public class GetInterstielAdHistoryModelsQuery : IRequest<IDataResult<IEnumerable<InterstielAdHistoryModel>>>
    {
        public class GetInterstielAdHistoryModelsQueryHandler : IRequestHandler<GetInterstielAdHistoryModelsQuery, IDataResult<IEnumerable<InterstielAdHistoryModel>>>
        {
            private readonly IInterstielAdHistoryModelRepository _interstielAdHistoryModelRepository;
            private readonly IMediator _mediator;

            public GetInterstielAdHistoryModelsQueryHandler(IInterstielAdHistoryModelRepository interstielAdHistoryModelRepository, IMediator mediator)
            {
                _interstielAdHistoryModelRepository = interstielAdHistoryModelRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<InterstielAdHistoryModel>>> Handle(GetInterstielAdHistoryModelsQuery request, CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<InterstielAdHistoryModel>>(await _interstielAdHistoryModelRepository.GetListAsync());
            }
        }
    }
}