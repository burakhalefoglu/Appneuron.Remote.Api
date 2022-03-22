using Business.BusinessAspects;
using Business.Constants;
using Business.Handlers.RemoteOfferProductModels.ValidationRules;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;

namespace Business.Handlers.RemoteOfferProductModels.Commands;

public class DeleteRemoteOfferProductModelCommand : IRequest<IResult>
{
    public string RemoteOfferName { get; set; }
    public string Version { get; set; }
    public long ProjectId { get; set; }
    public string Name { get; set; }
    public byte[] Image { get; set; }
    public float Count { get; set; }
    public string ImageName { get; set; }

    public class
        DeleteRemoteOfferProductModelCommandHandler : IRequestHandler<DeleteRemoteOfferProductModelCommand, IResult>
    {
        private readonly IRemoteOfferProductModelRepository _remoteOfferProductModelRepository;

        public DeleteRemoteOfferProductModelCommandHandler(
            IRemoteOfferProductModelRepository remoteOfferProductModelRepository)
        {
            _remoteOfferProductModelRepository = remoteOfferProductModelRepository;
        }

        [ValidationAspect(typeof(RemoteOfferProductModelValidator), Priority = 2)]
        [CacheRemoveAspect("Get")]
        [LogAspect(typeof(ConsoleLogger))]
        [SecuredOperation(Priority = 1)]
        public async Task<IResult> Handle(DeleteRemoteOfferProductModelCommand request,
            CancellationToken cancellationToken)
        {
            await _remoteOfferProductModelRepository.DeleteAsync(new RemoteOfferProductModel
            {
                Count = request.Count,
                Image = request.Image,
                Name = request.Name,
                Version = request.Version,
                ProjectId = request.ProjectId,
                ImageName = request.ImageName,
                RemoteOfferName = request.RemoteOfferName
            });

            return new SuccessResult(Messages.Deleted);
        }
    }
}