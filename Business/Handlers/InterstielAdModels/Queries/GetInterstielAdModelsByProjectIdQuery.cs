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

namespace Business.Handlers.InterstielAdModels.Queries
{
    public class GetInterstielAdModelsByProjectIdQuery : IRequest<IDataResult<IEnumerable<InterstielAdModel>>>
    {
        public string ProjectId { get; set; }

        public class GetInterstielAdModelsByProjectIdQueryHandler : IRequestHandler<
            GetInterstielAdModelsByProjectIdQuery, IDataResult<IEnumerable<InterstielAdModel>>>
        {
            private readonly IInterstielAdModelRepository _interstielAdModelRepository;
            private readonly IMediator _mediator;

            public GetInterstielAdModelsByProjectIdQueryHandler(
                IInterstielAdModelRepository interstielAdModelRepository, IMediator mediator)
            {
                _interstielAdModelRepository = interstielAdModelRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(ConsoleLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<InterstielAdModel>>> Handle(
                GetInterstielAdModelsByProjectIdQuery request, CancellationToken cancellationToken)
            {
                var result = await _interstielAdModelRepository.GetListAsync(i => i.ProjectId == request.ProjectId);
                return new SuccessDataResult<IEnumerable<InterstielAdModel>>(result);
            }
        }
    }
}