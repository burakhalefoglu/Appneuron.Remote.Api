
using System;
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
using Business.Handlers.InterstielAdHistoryModels.ValidationRules;

namespace Business.Handlers.InterstielAdHistoryModels.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateInterstielAdHistoryModelCommand : IRequest<IResult>
    {

        public string Name { get; set; }
        public string ProjectId { get; set; } 
        public float Version { get; set; }
        public int playerPercent { get; set; }
        public bool IsAdvSettingsActive { get; set; }


        public class CreateInterstielAdHistoryModelCommandHandler : IRequestHandler<CreateInterstielAdHistoryModelCommand, IResult>
        {
            private readonly IInterstielAdHistoryModelRepository _interstielAdHistoryModelRepository;
            private readonly IMediator _mediator;
            public CreateInterstielAdHistoryModelCommandHandler(IInterstielAdHistoryModelRepository interstielAdHistoryModelRepository, IMediator mediator)
            {
                _interstielAdHistoryModelRepository = interstielAdHistoryModelRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateInterstielAdHistoryModelValidator))]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateInterstielAdHistoryModelCommand request, CancellationToken cancellationToken)
            {

                var addedInterstielAdHistoryModel = new InterstielAdHistoryModel
                {
                    Name = request.Name,
                    ProjectId = request.ProjectId,
                    Version = request.Version,
                    playerPercent = request.playerPercent,
                    IsAdvSettingsActive = request.IsAdvSettingsActive,
                    StarTime = DateTime.Now
                };

                await _interstielAdHistoryModelRepository.AddAsync(addedInterstielAdHistoryModel);

                return new SuccessResult(Messages.Added);
            }
        }
    }
}