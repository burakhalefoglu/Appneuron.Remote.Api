
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

namespace Business.Handlers.InterstielAdModels.Queries
{
    public class GetInterstielAdModelQuery : IRequest<IDataResult<InterstielAdModel>>
    {
        public string ObjectId { get; set; }
        private ObjectId Id => new ObjectId(this.ObjectId);

        public class GetInterstielAdModelQueryHandler : IRequestHandler<GetInterstielAdModelQuery, IDataResult<InterstielAdModel>>
        {
            private readonly IInterstielAdModelRepository _interstielAdModelRepository;
            private readonly IMediator _mediator;

            public GetInterstielAdModelQueryHandler(IInterstielAdModelRepository interstielAdModelRepository, IMediator mediator)
            {
                _interstielAdModelRepository = interstielAdModelRepository;
                _mediator = mediator;
            }
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<InterstielAdModel>> Handle(GetInterstielAdModelQuery request, CancellationToken cancellationToken)
            {
                var interstielAdModel = await _interstielAdModelRepository.GetByIdAsync(request.Id);
                return new SuccessDataResult<InterstielAdModel>(interstielAdModel);
            }
        }
    }
}
