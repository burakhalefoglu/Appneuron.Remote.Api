using System;
using System.Threading;
using System.Threading.Tasks;
using Business.BusinessAspects;
using Business.Constants;
using Business.Handlers.InterstielAdHistoryModels.ValidationRules;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;

namespace Business.Handlers.InterstitialAdHistoryModels.Commands
{
    /// <summary>
    /// </summary>
    public class CreateInterstitialAdHistoryModelCommand : IRequest<IResult>
    {
        public string Name { get; set; }
        public string ProjectId { get; set; }
        public string Version { get; set; }
        public int PlayerPercent { get; set; }
        public bool IsAdvSettingsActive { get; set; }


        public class
            CreateInterstitialAdHistoryModelCommandHandler : IRequestHandler<CreateInterstitialAdHistoryModelCommand,
                IResult>
        {
            private readonly IInterstielAdHistoryModelRepository _interstitialAdHistoryModelRepository;

            public CreateInterstitialAdHistoryModelCommandHandler(
                IInterstielAdHistoryModelRepository interstitialAdHistoryModelRepository)
            {
                _interstitialAdHistoryModelRepository = interstitialAdHistoryModelRepository;
            }

            [ValidationAspect(typeof(CreateInterstielAdHistoryModelValidator))]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(ConsoleLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateInterstitialAdHistoryModelCommand request,
                CancellationToken cancellationToken)
            {
                var addedInterstitialAdHistoryModel = new InterstielAdHistoryModel
                {
                    Name = request.Name,
                    ProjectId = request.ProjectId,
                    Version = request.Version,
                    playerPercent = request.PlayerPercent,
                    IsAdvSettingsActive = request.IsAdvSettingsActive,
                    StarTime = DateTime.Now
                };

                await _interstitialAdHistoryModelRepository.AddAsync(addedInterstitialAdHistoryModel);

                return new SuccessResult(Messages.Added);
            }
        }
    }
}