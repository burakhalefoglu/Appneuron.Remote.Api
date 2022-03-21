using System.Threading;
using System.Threading.Tasks;
using Business.BusinessAspects;
using Business.Constants;
using Business.Handlers.AdvStrategies.Command;
using Business.Handlers.InterstitialAdModels.ValidationRules;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.MessageBrokers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using MediatR;

namespace Business.Handlers.InterstitialAdModels.Commands
{
    /// <summary>
    /// </summary>
    public class CreateInterstitialAdModelCommand : IRequest<IResult>
    {
        public long ProjectId { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public int PlayerPercent { get; set; }
        public AdvStrategy[] AdvStrategies { get; set; }


        public class
            CreateInterstitialAdModelCommandHandler : IRequestHandler<CreateInterstitialAdModelCommand, IResult>
        {
            private readonly IInterstielAdModelRepository _interstitialAdModelRepository;
            private readonly IMediator _mediator;

            public CreateInterstitialAdModelCommandHandler(IInterstielAdModelRepository interstitialAdModelRepository,
                IMediator mediator)
            {
                _interstitialAdModelRepository = interstitialAdModelRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateInterstielAdModelValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(ConsoleLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateInterstitialAdModelCommand request,
                CancellationToken cancellationToken)
            {
                var isThereInterstitialAdModelRecord = await _interstitialAdModelRepository.AnyAsync(u =>
                    u.Name == request.Name && u.ProjectId == request.ProjectId && u.Version == request.Version &&
                    u.Status == true);

                if (isThereInterstitialAdModelRecord)
                    return new ErrorResult(Messages.AlreadyExist);

                var addedInterstitialAdModel = new InterstitialAdModel
                {
                    ProjectId = request.ProjectId,
                    Name = request.Name,
                    Version = request.Version,
                    PlayerPercent = request.PlayerPercent,
                };

                await _interstitialAdModelRepository.AddAsync(addedInterstitialAdModel);

                foreach (var advStrategy in request.AdvStrategies)
                {
                    await _mediator.Send(new CreateAdvStrategyCommand
                    {
                        Count = advStrategy.StrategyCount,
                        Name = advStrategy.Name,
                    }, cancellationToken);
                }

                return new SuccessResult(Messages.Added);
            }
        }
    }
}