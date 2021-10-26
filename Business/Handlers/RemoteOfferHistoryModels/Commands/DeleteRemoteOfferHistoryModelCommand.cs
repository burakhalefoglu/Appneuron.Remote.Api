
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Business.BusinessAspects;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace Business.Handlers.RemoteOfferHistoryModels.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class DeleteRemoteOfferHistoryModelCommand : IRequest<IResult>
    {
        public string ObjectId { get; set; }
        private ObjectId Id => new ObjectId(this.ObjectId);

        public class DeleteRemoteOfferHistoryModelCommandHandler : IRequestHandler<DeleteRemoteOfferHistoryModelCommand, IResult>
        {
            private readonly IRemoteOfferHistoryModelRepository _remoteOfferHistoryModelRepository;
            private readonly IMediator _mediator;

            public DeleteRemoteOfferHistoryModelCommandHandler(IRemoteOfferHistoryModelRepository remoteOfferHistoryModelRepository, IMediator mediator)
            {
                _remoteOfferHistoryModelRepository = remoteOfferHistoryModelRepository;
                _mediator = mediator;
            }

            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(DeleteRemoteOfferHistoryModelCommand request, CancellationToken cancellationToken)
            {


                await _remoteOfferHistoryModelRepository.DeleteAsync(request.Id);

                return new SuccessResult(Messages.Deleted);
            }
        }
    }
}

