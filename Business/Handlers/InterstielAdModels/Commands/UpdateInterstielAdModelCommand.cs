
using System;
using Business.Constants;
using Business.BusinessAspects;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Business.Handlers.InterstielAdHistoryModels.Commands;
using Core.Aspects.Autofac.Validation;
using Business.Handlers.InterstielAdModels.ValidationRules;
using Core.Aspects.Autofac.Transaction;

namespace Business.Handlers.InterstielAdModels.Commands
{


    public class UpdateInterstielAdModelCommand : IRequest<IResult>
    {
        public string ProjectId { get; set; }
        public string Name { get; set; }
        public float Version { get; set; }
        public int PlayerPercent { get; set; }
        public bool IsAdvSettingsActive { get; set; }

        public class UpdateInterstielAdModelCommandHandler : IRequestHandler<UpdateInterstielAdModelCommand, IResult>
        {
            private readonly IInterstielAdModelRepository _interstielAdModelRepository;
            private readonly IMediator _mediator;

            public UpdateInterstielAdModelCommandHandler(IInterstielAdModelRepository interstielAdModelRepository, IMediator mediator)
            {
                _interstielAdModelRepository = interstielAdModelRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdateInterstielAdModelValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            [TransactionScopeAspectAsync]
            public async Task<IResult> Handle(UpdateInterstielAdModelCommand request, CancellationToken cancellationToken)
            {
                var isValid = _interstielAdModelRepository.Any( i => 
                    i.ProjectId == request.ProjectId && i.Name == request.Name &&
                    i.Version == request.Version);
                if(!isValid)
                {
                    return new ErrorResult(Messages.NoContent);
                }

                var resultData = await _interstielAdModelRepository.GetByFilterAsync(i => i.ProjectId == request.ProjectId && i.Name == request.Name &&
                   i.Version == request.Version);

                resultData.PlayerPercent = request.PlayerPercent;
                resultData.IsAdvSettingsActive = request.IsAdvSettingsActive;

                await _mediator.Send(new CreateInterstielAdHistoryModelCommand()
                {
                    IsAdvSettingsActive = resultData.IsAdvSettingsActive,
                    Name = resultData.Name,
                    Version = resultData.Version,
                    playerPercent = resultData.PlayerPercent,
                    ProjectId = request.ProjectId
                });

                await _interstielAdModelRepository.UpdateAsync(resultData,
                    i=> i.ProjectId == request.ProjectId && i.Name == request.Name &&
                    i.Version == request.Version);

                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

