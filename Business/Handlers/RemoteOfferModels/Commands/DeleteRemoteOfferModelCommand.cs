using System.Threading;
using System.Threading.Tasks;
using Business.BusinessAspects;
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using MediatR;

namespace Business.Handlers.RemoteOfferModels.Commands
{
    /// <summary>
    /// </summary>
    public class DeleteRemoteOfferModelCommand : IRequest<IResult>
    {
        public string ProjectId { get; set; }
        public string Name { get; set; }
        public int Version { get; set; }

        public class DeleteRemoteOfferModelCommandHandler : IRequestHandler<DeleteRemoteOfferModelCommand, IResult>
        {
            private readonly IMediator _mediator;
            private readonly IRemoteOfferModelRepository _remoteOfferModelRepository;

            public DeleteRemoteOfferModelCommandHandler(IRemoteOfferModelRepository remoteOfferModelRepository,
                IMediator mediator)
            {
                _remoteOfferModelRepository = remoteOfferModelRepository;
                _mediator = mediator;
            }

            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(ConsoleLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(DeleteRemoteOfferModelCommand request,
                CancellationToken cancellationToken)
            {
                await _remoteOfferModelRepository.DeleteAsync(i => i.ProjectId == request.ProjectId &&
                                                                   i.Name == request.Name &&
                                                                   i.Version == request.Version);

                return new SuccessResult(Messages.Deleted);
            }
        }
    }
}