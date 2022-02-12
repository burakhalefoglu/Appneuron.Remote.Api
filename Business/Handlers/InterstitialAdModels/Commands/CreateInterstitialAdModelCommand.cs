using System.Threading;
using System.Threading.Tasks;
using Business.BusinessAspects;
using Business.Constants;
using Business.Handlers.AdvStrategies.Command;
using Business.Handlers.InterstielAdModels.ValidationRules;
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
        public bool IsAdvSettingsActive { get; set; }
        public AdvStrategy[] AdvStrategies { get; set; }


        public class
            CreateInterstitialAdModelCommandHandler : IRequestHandler<CreateInterstitialAdModelCommand, IResult>
        {
            private readonly IInterstielAdModelRepository _interstitialAdModelRepository;
            private readonly IMessageBroker _messageBroker;
            private readonly IMediator _mediator;


            public CreateInterstitialAdModelCommandHandler(IInterstielAdModelRepository interstitialAdModelRepository,
                IMessageBroker messageBroker, IMediator mediator)
            {
                _interstitialAdModelRepository = interstitialAdModelRepository;
                _messageBroker = messageBroker;
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
                    IsAdvSettingsActive = request.IsAdvSettingsActive,
                };

                await _interstitialAdModelRepository.AddAsync(addedInterstitialAdModel);

                foreach (var advStrategy in request.AdvStrategies)
                {
                    await _mediator.Send(new CreateAdvStrategyCommand
                    {
                        Count = advStrategy.Count,
                        Name = advStrategy.Name
                    }, cancellationToken);
                }
                
                await _messageBroker.SendMessageAsync(new InterstitialAdModelDto
                {
                    ProjectId = request.ProjectId,
                    Name = request.Name,
                    Version = request.Version,
                    PlayerPercent = request.PlayerPercent,
                    IsAdvSettingsActive = request.IsAdvSettingsActive,
                    AdvStrategies = request.AdvStrategies
                    
                });

                return new SuccessResult(Messages.Added);
            }
        }
    }
}