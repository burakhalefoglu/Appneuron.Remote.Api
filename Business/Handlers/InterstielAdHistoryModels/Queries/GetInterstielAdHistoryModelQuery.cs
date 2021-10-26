
using Business.BusinessAspects;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using MongoDB.Bson;

namespace Business.Handlers.InterstielAdHistoryModels.Queries
{
    public class GetInterstielAdHistoryModelQuery : IRequest<IDataResult<InterstielAdHistoryModel>>
    {
        public string ObjectId { get; set; }
        private ObjectId Id => new ObjectId(this.ObjectId);

        public class GetInterstielAdHistoryModelQueryHandler : IRequestHandler<GetInterstielAdHistoryModelQuery, IDataResult<InterstielAdHistoryModel>>
        {
            private readonly IInterstielAdHistoryModelRepository _interstielAdHistoryModelRepository;
            private readonly IMediator _mediator;

            public GetInterstielAdHistoryModelQueryHandler(IInterstielAdHistoryModelRepository interstielAdHistoryModelRepository, IMediator mediator)
            {
                _interstielAdHistoryModelRepository = interstielAdHistoryModelRepository;
                _mediator = mediator;
            }
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<InterstielAdHistoryModel>> Handle(GetInterstielAdHistoryModelQuery request, CancellationToken cancellationToken)
            {
                var interstielAdHistoryModel = await _interstielAdHistoryModelRepository.GetByIdAsync(request.Id);
                return new SuccessDataResult<InterstielAdHistoryModel>(interstielAdHistoryModel);
            }
        }
    }
}
