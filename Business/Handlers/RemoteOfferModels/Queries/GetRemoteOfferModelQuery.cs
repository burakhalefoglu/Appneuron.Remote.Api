
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

namespace Business.Handlers.RemoteOfferModels.Queries
{
    public class GetRemoteOfferModelQuery : IRequest<IDataResult<RemoteOfferModel>>
    {
        public string ObjectId { get; set; }
        private ObjectId Id => new ObjectId(this.ObjectId);

        public class GetRemoteOfferModelQueryHandler : IRequestHandler<GetRemoteOfferModelQuery, IDataResult<RemoteOfferModel>>
        {
            private readonly IRemoteOfferModelRepository _remoteOfferModelRepository;
            private readonly IMediator _mediator;

            public GetRemoteOfferModelQueryHandler(IRemoteOfferModelRepository remoteOfferModelRepository, IMediator mediator)
            {
                _remoteOfferModelRepository = remoteOfferModelRepository;
                _mediator = mediator;
            }
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<RemoteOfferModel>> Handle(GetRemoteOfferModelQuery request, CancellationToken cancellationToken)
            {
                var remoteOfferModel = await _remoteOfferModelRepository.GetByIdAsync(request.Id);
                return new SuccessDataResult<RemoteOfferModel>(remoteOfferModel);
            }
        }
    }
}
