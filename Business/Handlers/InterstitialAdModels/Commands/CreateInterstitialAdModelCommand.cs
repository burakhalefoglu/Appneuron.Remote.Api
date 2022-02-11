using System.Threading;
using System.Threading.Tasks;
using Business.BusinessAspects;
using Business.Constants;
using Business.Handlers.InterstielAdModels.ValidationRules;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
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


        public class CreateInterstitialAdModelCommandHandler : IRequestHandler<CreateInterstitialAdModelCommand, IResult>
        {
            private readonly IInterstielAdModelRepository _interstitialAdModelRepository;

            public CreateInterstitialAdModelCommandHandler(IInterstielAdModelRepository interstitialAdModelRepository)
            {
                _interstitialAdModelRepository = interstitialAdModelRepository;
            }

            [ValidationAspect(typeof(CreateInterstielAdModelValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(ConsoleLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateInterstitialAdModelCommand request,
                CancellationToken cancellationToken)
            {
                var isThereInterstitialAdModelRecord = _interstitialAdModelRepository.Any(u =>
                    u.Name == request.Name && u.ProjectId == request.ProjectId && u.Version == request.Version && u.Status == true);

                if (isThereInterstitialAdModelRecord)
                    return new ErrorResult(Messages.AlreadyExist);

                var addedInterstitialAdModel = new InterstitialAdModel
                {
                    ProjectId = request.ProjectId,
                    Name = request.Name,
                    Version = request.Version,
                    PlayerPercent = request.PlayerPercent,
                    IsAdvSettingsActive = request.IsAdvSettingsActive,
                    AdvStrategies = request.AdvStrategies
                };

                await _interstitialAdModelRepository.AddAsync(addedInterstitialAdModel);

                return new SuccessResult(Messages.Added);
            }
        }
    }
}