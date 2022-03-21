using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Business.BusinessAspects;
using Business.Constants;
using Business.Handlers.RemoteOfferModels.ValidationRules;
using Business.Handlers.RemoteOfferProductModels.Queries;
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

namespace Business.Handlers.RemoteOfferModels.Commands
{
    public class UpdateRemoteOfferModelCommand : IRequest<IResult>
    {
        public long ProjectId { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public int PlayerPercent { get; set; }
        public bool Status { get; set; }

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
                    _remoteOfferModelRepository.GetAsync(r =>r.ProjectId == request.ProjectId &&
                                                             r.Name == request.Name &&
                                                             r.Version == request.Version &&
                                                             r.Terminated == false);
                if (resultData is null) return new ErrorResult(Messages.NoContent);

                resultData.PlayerPercent = request.PlayerPercent;
                resultData.Status = request.Status;
                
                if (resultData.Status)
                {
                    resultData.StartTime = DateTime.Now.Ticks;
                    resultData.FinishTime = DateTime.Now.AddHours(resultData.ValidityPeriod).Ticks;
                }
                await _remoteOfferModelRepository.UpdateAsync(resultData);

                return new SuccessResult(Messages.Updated);
            }
        }
    }
}