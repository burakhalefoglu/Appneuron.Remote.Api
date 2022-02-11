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

namespace Business.Handlers.InterstielAdModels.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class DeleteInterstielAdModelCommand : IRequest<IResult>
    {
        public long ProjectId { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }

        public class DeleteInterstitialAdModelCommandHandler : IRequestHandler<DeleteInterstielAdModelCommand, IResult>
        {
            private readonly IInterstielAdModelRepository _interstielAdModelRepository;
            private readonly IMediator _mediator;

            public DeleteInterstitialAdModelCommandHandler(IInterstielAdModelRepository interstielAdModelRepository, IMediator mediator)
            {
                _interstielAdModelRepository = interstielAdModelRepository;
                _mediator = mediator;
            }

            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(LogstashLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(DeleteInterstielAdModelCommand request, CancellationToken cancellationToken)
            {
                var isThereInterstitialAdModelRecord = await _interstielAdModelRepository.GetAsync(u =>
                    u.Name == request.Name && u.ProjectId == request.ProjectId && u.Version == request.Version && u.Status == true);

                if (isThereInterstitialAdModelRecord is null)
                    return new ErrorResult(Messages.NotFound);

                isThereInterstitialAdModelRecord.Status = false;
                
                await _interstielAdModelRepository.UpdateAsync(isThereInterstitialAdModelRecord);
                return new SuccessResult(Messages.Deleted);
            }
        }
    }
}

