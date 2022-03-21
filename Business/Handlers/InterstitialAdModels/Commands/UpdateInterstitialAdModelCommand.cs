using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Business.BusinessAspects;
using Business.Constants;
using Business.Handlers.AdvStrategies.Query;
using Business.Handlers.InterstitialAdModels.ValidationRules;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.MessageBrokers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Dtos;
using MediatR;

namespace Business.Handlers.InterstitialAdModels.Commands
{
    public class UpdateInterstitialAdModelCommand : IRequest<IResult>
    {
        public long ProjectId { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public int PlayerPercent { get; set; }

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
                var isValid = await _interstitialAdModelRepository.AnyAsync(i =>
                    i.ProjectId == request.ProjectId && i.Name == request.Name &&
                    i.Version == request.Version && i.Status == true);
                if (!isValid) return new ErrorResult(Messages.NoContent);

                var resultData = await _interstitialAdModelRepository.GetAsync(u =>
                    u.Name == request.Name &&
                    u.ProjectId == request.ProjectId &&
                    u.Version == request.Version &&
                    u.Status == true);

                resultData.PlayerPercent = request.PlayerPercent;

                await _interstitialAdModelRepository.UpdateAsync(resultData);
                return new SuccessResult(Messages.Updated);
            }
        }
    }
}