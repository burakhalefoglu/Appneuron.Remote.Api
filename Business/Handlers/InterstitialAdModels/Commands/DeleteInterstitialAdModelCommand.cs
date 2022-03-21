using Business.BusinessAspects;
using Business.Constants;
using Business.Handlers.AdvStrategies.Command;
using Business.Handlers.AdvStrategies.Query;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using MediatR;

namespace Business.Handlers.InterstitialAdModels.Commands;

    /// <summary>
    /// </summary>
    public class DeleteInterstitialAdModelCommand : IRequest<IResult>
    {
        public long ProjectId { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }

        public class DeleteInterstitialAdModelCommandHandler : IRequestHandler<DeleteInterstitialAdModelCommand, IResult>
        {
            private readonly IInterstielAdModelRepository _interstitialAdModelRepository;
            private readonly IMediator _mediator;

            public DeleteInterstitialAdModelCommandHandler(IInterstielAdModelRepository interstitialAdModelRepository,
                IMediator mediator)
            {
                _interstitialAdModelRepository = interstitialAdModelRepository;
                _mediator = mediator;
            }

            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(ConsoleLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(DeleteInterstitialAdModelCommand request,
                CancellationToken cancellationToken)
            {
                var isThereInterstitialAdModelRecord = await _interstitialAdModelRepository.GetAsync(u =>
                    u.Name == request.Name &&
                    u.ProjectId == request.ProjectId &&
                    u.Version == request.Version &&
                    u.Status == true);

                if (isThereInterstitialAdModelRecord is null)
                    return new ErrorResult(Messages.NotFound);
                
                isThereInterstitialAdModelRecord.IsActive = false;
                await _interstitialAdModelRepository.DeleteAsync(isThereInterstitialAdModelRecord);
                
                var advStrategies = (await _mediator.Send(new GetAdvStrategyQuery
                {
                    Name = request.Name,
                    Version = request.Version,
                    ProjectId = request.ProjectId
                }, cancellationToken)).Data.ToList();
                foreach (var advStrategy in advStrategies)
                {
                    await _mediator.Send(new DeleteAdvStrategyCommand
                    {
                        Count = advStrategy.StrategyValue,
                        Name = advStrategy.Name,
                        ProjectId = advStrategy.ProjectId

                    }, cancellationToken);
                }
                return new SuccessResult(Messages.Deleted);
            }
        }
    }