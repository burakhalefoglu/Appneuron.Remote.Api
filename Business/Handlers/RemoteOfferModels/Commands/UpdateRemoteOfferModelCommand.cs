using Business.BusinessAspects;
using Business.Constants;
using Business.Handlers.RemoteOfferModels.ValidationRules;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using MediatR;

namespace Business.Handlers.RemoteOfferModels.Commands;

public class UpdateRemoteOfferModelCommand : IRequest<IResult>
{
    public long ProjectId { get; set; }
    public string Name { get; set; }
    public string Version { get; set; }
    public int PlayerPercent { get; set; }
    public bool IsActive { get; set; }

    public class UpdateRemoteOfferModelCommandHandler : IRequestHandler<UpdateRemoteOfferModelCommand, IResult>
    {
        private readonly IMediator _mediator;
        private readonly IRemoteOfferModelRepository _remoteOfferModelRepository;

        public UpdateRemoteOfferModelCommandHandler(IRemoteOfferModelRepository remoteOfferModelRepository,
            IMediator mediator)
        {
            _remoteOfferModelRepository = remoteOfferModelRepository;
            _mediator = mediator;
        }

        [ValidationAspect(typeof(UpdateRemoteOfferModelValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        [LogAspect(typeof(ConsoleLogger))]
        [TransactionScopeAspect]
        [SecuredOperation(Priority = 1)]
        public async Task<IResult> Handle(UpdateRemoteOfferModelCommand request,
            CancellationToken cancellationToken)
        {
            var resultData = await
                _remoteOfferModelRepository.GetAsync(r => r.ProjectId == request.ProjectId &&
                                                          r.Name == request.Name &&
                                                          r.Version == request.Version &&
                                                          r.Status == true);
            if (resultData is null) return new ErrorResult(Messages.NoContent);

            resultData.PlayerPercent = request.PlayerPercent;
            resultData.IsActive = request.IsActive;

            if (resultData.IsActive)
            {
                resultData.StartTime = DateTime.Now.Ticks;
                resultData.FinishTime = DateTime.Now.AddHours(resultData.ValidityPeriod).Ticks;
            }

            await _remoteOfferModelRepository.UpdateAsync(resultData);

            return new SuccessResult(Messages.Updated);
        }
    }
}