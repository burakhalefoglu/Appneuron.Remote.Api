using Business.BusinessAspects;
using Business.Constants;
using Business.Handlers.InterstitialAdModels.ValidationRules;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using MediatR;

namespace Business.Handlers.InterstitialAdModels.Commands;

public class UpdateInterstitialAdModelCommand : IRequest<IResult>
{
    public long ProjectId { get; set; }
    public string Name { get; set; }
    public string Version { get; set; }
    public int PlayerPercent { get; set; }
    public bool IsActive { get; set; }

    public class
        UpdateInterstitialAdModelCommandHandler : IRequestHandler<UpdateInterstitialAdModelCommand, IResult>
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
        [TransactionScopeAspect]
        public async Task<IResult> Handle(UpdateInterstitialAdModelCommand request,
            CancellationToken cancellationToken)
        {
            var resultData = await _interstitialAdModelRepository.GetAsync(u =>
                u.Name == request.Name &&
                u.ProjectId == request.ProjectId &&
                u.Version == request.Version &&
                u.Status == true);
            if (resultData is null) return new ErrorResult(Messages.NoContent);

            resultData.PlayerPercent = request.PlayerPercent;
            resultData.IsActive = request.IsActive;

            await _interstitialAdModelRepository.UpdateAsync(resultData);
            return new SuccessResult(Messages.Updated);
        }
    }
}