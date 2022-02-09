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
        public string Version { get; set; }

        public class DeleteRemoteOfferModelCommandHandler : IRequestHandler<DeleteRemoteOfferModelCommand, IResult>
        {
            private readonly IRemoteOfferModelRepository _remoteOfferModelRepository;

            public DeleteRemoteOfferModelCommandHandler(IRemoteOfferModelRepository remoteOfferModelRepository)
            {
                _remoteOfferModelRepository = remoteOfferModelRepository;
            }

            [CacheRemoveAspect("Get")]
<<<<<<< Updated upstream
            [LogAspect(typeof(ConsoleLogger))]
=======
            [LogAspect(typeof(LogstashLogger))]
>>>>>>> Stashed changes
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(DeleteRemoteOfferModelCommand request,
                CancellationToken cancellationToken)
            {
                var isThereInterstitialAdModelRecord = await _remoteOfferModelRepository.GetAsync(u =>
                    u.Name == request.Name &&
                    u.ProjectId == request.ProjectId &&
                    u.Version == request.Version &&
                    u.Status == true);

                if (isThereInterstitialAdModelRecord is null)
                    return new ErrorResult(Messages.NotFound);
                isThereInterstitialAdModelRecord.Status = false;
                await _remoteOfferModelRepository.UpdateAsync(isThereInterstitialAdModelRecord, i 
                    => i.ProjectId == request.ProjectId &&
                       i.Name == request.Name &&
                       i.Version == request.Version);

                return new SuccessResult(Messages.Deleted);
            }
        }
    }
}