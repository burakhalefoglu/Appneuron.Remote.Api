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

namespace Business.Handlers.InterstielAdHistoryModels.Queries
{
    public class
        GetInterstielAdHistoryModelByProjectIdQuery : IRequest<IDataResult<IEnumerable<InterstielAdHistoryModel>>>
    {
        public string ProjectId { get; set; }

        public class GetInterstielAdHistoryModelByProjectIdQueryHandler : IRequestHandler<
            GetInterstielAdHistoryModelByProjectIdQuery, IDataResult<IEnumerable<InterstielAdHistoryModel>>>
        {
            private readonly IInterstielAdHistoryModelRepository _interstielAdHistoryModelRepository;
            private readonly IMediator _mediator;

            public GetInterstielAdHistoryModelByProjectIdQueryHandler(
                IInterstielAdHistoryModelRepository interstielAdHistoryModelRepository, IMediator mediator)
            {
                _interstielAdHistoryModelRepository = interstielAdHistoryModelRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(ConsoleLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<InterstielAdHistoryModel>>> Handle(
                GetInterstielAdHistoryModelByProjectIdQuery request, CancellationToken cancellationToken)
            {
                var result = await _interstielAdHistoryModelRepository
                    .GetListAsync(r => r.ProjectId == request.ProjectId);

                return new SuccessDataResult<IEnumerable<InterstielAdHistoryModel>>(result);
            }
        }
    }
}