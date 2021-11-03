
using Business.BusinessAspects;
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Business.Handlers.InterstitialAdEventModels.ValidationRules;
using Business.MessageBrokers.Kafka;

namespace Business.Handlers.InterstitialAdEventModels.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateInterstitialAdEventModelCommand : IRequest<IResult>
    {

        public string ProjectId { get; set; }
        public string[] ClientIdList { get; set; }
        public bool IsAdvSettingsActive { get; set; }
        public System.Collections.Generic.Dictionary<string, int> AdvFrequencyStrategies { get; set; }


        public class CreateInterstitialAdEventModelCommandHandler : IRequestHandler<CreateInterstitialAdEventModelCommand, IResult>
        {
            private readonly IInterstitialAdEventModelRepository _interstitialAdEventModelRepository;
            private readonly IMediator _mediator;
            private readonly IKafkaMessageBroker _kafkaMessageBroker; 

            public CreateInterstitialAdEventModelCommandHandler(
                IInterstitialAdEventModelRepository interstitialAdEventModelRepository,
                IMediator mediator,
                IKafkaMessageBroker kafkaMessageBroker)
            {
                _interstitialAdEventModelRepository = interstitialAdEventModelRepository;
                _mediator = mediator;
                _kafkaMessageBroker = kafkaMessageBroker;

            }

            [ValidationAspect(typeof(CreateInterstitialAdEventModelValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateInterstitialAdEventModelCommand request, CancellationToken cancellationToken)
            {
                var addedInterstitialAdEventModel = new InterstitialAdEventModel
                {
                    ProjectId = request.ProjectId,
                    ClientIdList = request.ClientIdList,
                    IsAdvSettingsActive = request.IsAdvSettingsActive,
                    AdvFrequencyStrategies = request.AdvFrequencyStrategies,

                };
                
                await _interstitialAdEventModelRepository.AddAsync(addedInterstitialAdEventModel);
                //await _kafkaMessageBroker.SendMessageAsync(addedInterstitialAdEventModel);

                return new SuccessResult(Messages.Added);
            }
        }
    }
}