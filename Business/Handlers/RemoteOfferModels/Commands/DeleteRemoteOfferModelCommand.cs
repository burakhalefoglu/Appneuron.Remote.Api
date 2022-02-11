using System.Threading;
using System.Threading.Tasks;
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
        public long ProjectId { get; set; }
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
            [LogAspect(typeof(ConsoleLogger))]
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
                await _remoteOfferModelRepository.UpdateAsync(isThereInterstitialAdModelRecord);

                return new SuccessResult(Messages.Deleted);
            }
        }
    }
}