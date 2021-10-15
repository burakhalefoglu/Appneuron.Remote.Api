
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

namespace Business.Handlers.InterstielAdModels.Queries
{

    public class GetInterstielAdModelsByProjectIdQuery : IRequest<IDataResult<IEnumerable<InterstielAdModel>>>
    {
        public string ProjectId { get; set; }
        public class GetInterstielAdModelsByProjectIdQueryHandler : IRequestHandler<GetInterstielAdModelsByProjectIdQuery, IDataResult<IEnumerable<InterstielAdModel>>>
        {
            private readonly IInterstielAdModelRepository _interstielAdModelRepository;
            private readonly IMediator _mediator;

            public GetInterstielAdModelsByProjectIdQueryHandler(IInterstielAdModelRepository interstielAdModelRepository, IMediator mediator)
            {
                _interstielAdModelRepository = interstielAdModelRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<InterstielAdModel>>> Handle(GetInterstielAdModelsByProjectIdQuery request, CancellationToken cancellationToken)
            {
                var result = await _interstielAdModelRepository.GetListAsync(i=> i.ProjectId == request.ProjectId);
                return new SuccessDataResult<IEnumerable<InterstielAdModel>>(result);
            }
        }
    }
}