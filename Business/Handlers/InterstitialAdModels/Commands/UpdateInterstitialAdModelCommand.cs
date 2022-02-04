using System.Threading;
using System.Threading.Tasks;
using Business.BusinessAspects;
using Business.Constants;
using Business.Handlers.InterstielAdModels.ValidationRules;
using Business.Handlers.InterstitialAdHistoryModels.Commands;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using MediatR;

namespace Business.Handlers.InterstitialAdModels.Commands
{
    public class UpdateInterstitialAdModelCommand : IRequest<IResult>
    {
        public string ProjectId { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public int PlayerPercent { get; set; }
        public bool IsAdvSettingsActive { get; set; }

        public class UpdateInterstitialAdModelCommandHandler : IRequestHandler<UpdateInterstitialAdModelCommand, IResult>
        {
            private readonly IInterstielAdModelRepository _interstitialAdModelRepository;
            private readonly IMediator _mediator;

            public UpdateInterstitialAdModelCommandHandler(IInterstielAdModelRepository interstitialAdModelRepository,
                IMediator mediator)
            {
                _interstitialAdModelRepository = interstitialAdModelRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdateInterstielAdModelValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(ConsoleLogger))]
            [SecuredOperation(Priority = 1)]
            [TransactionScopeAspectAsync]
            public async Task<IResult> Handle(UpdateInterstitialAdModelCommand request,
                CancellationToken cancellationToken)
            {
                var isValid = await _interstitialAdModelRepository.AnyAsync(i =>
                    i.ProjectId == request.ProjectId && i.Name == request.Name &&
                    i.Version == request.Version && i.Status == true);
                if (!isValid) return new ErrorResult(Messages.NoContent);

                var resultData = await _interstitialAdModelRepository.GetAsync(i =>
                    i.ProjectId == request.ProjectId && i.Name == request.Name &&
                    i.Version == request.Version);

                resultData.PlayerPercent = request.PlayerPercent;
                resultData.IsAdvSettingsActive = request.IsAdvSettingsActive;

                await _mediator.Send(new CreateInterstitialAdHistoryModelCommand
                {
                    IsAdvSettingsActive = resultData.IsAdvSettingsActive,
                    Name = resultData.Name,
                    Version = resultData.Version,
                    PlayerPercent = resultData.PlayerPercent,
                    ProjectId = request.ProjectId
                }, cancellationToken);

                await _interstitialAdModelRepository.UpdateAsync(resultData,
                    i => i.ProjectId == request.ProjectId && i.Name == request.Name &&
                         i.Version == request.Version);

                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

