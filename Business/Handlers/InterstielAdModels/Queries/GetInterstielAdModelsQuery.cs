
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

namespace Business.Handlers.InterstielAdModels.Queries
{

    public class GetInterstielAdModelsQuery : IRequest<IDataResult<IEnumerable<InterstielAdModel>>>
    {
        public class GetInterstielAdModelsQueryHandler : IRequestHandler<GetInterstielAdModelsQuery, IDataResult<IEnumerable<InterstielAdModel>>>
        {
            private readonly IInterstielAdModelRepository _interstielAdModelRepository;
            private readonly IMediator _mediator;

            public GetInterstielAdModelsQueryHandler(IInterstielAdModelRepository interstielAdModelRepository, IMediator mediator)
            {
                _interstielAdModelRepository = interstielAdModelRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<InterstielAdModel>>> Handle(GetInterstielAdModelsQuery request, CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<InterstielAdModel>>(await _interstielAdModelRepository.GetListAsync());
            }
        }
    }
}