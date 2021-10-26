
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

namespace Business.Handlers.RemoteOfferHistoryModels.Queries
{
    public class GetRemoteOfferHistoryModelQuery : IRequest<IDataResult<RemoteOfferHistoryModel>>
    {
        public string ObjectId { get; set; }
        private ObjectId Id => new ObjectId(this.ObjectId);

        public class GetRemoteOfferHistoryModelQueryHandler : IRequestHandler<GetRemoteOfferHistoryModelQuery, IDataResult<RemoteOfferHistoryModel>>
        {
            private readonly IRemoteOfferHistoryModelRepository _remoteOfferHistoryModelRepository;
            private readonly IMediator _mediator;

            public GetRemoteOfferHistoryModelQueryHandler(IRemoteOfferHistoryModelRepository remoteOfferHistoryModelRepository, IMediator mediator)
            {
                _remoteOfferHistoryModelRepository = remoteOfferHistoryModelRepository;
                _mediator = mediator;
            }
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<RemoteOfferHistoryModel>> Handle(GetRemoteOfferHistoryModelQuery request, CancellationToken cancellationToken)
            {
                var remoteOfferHistoryModel = await _remoteOfferHistoryModelRepository.GetByIdAsync(request.Id);
                return new SuccessDataResult<RemoteOfferHistoryModel>(remoteOfferHistoryModel);
            }
        }
    }
}
