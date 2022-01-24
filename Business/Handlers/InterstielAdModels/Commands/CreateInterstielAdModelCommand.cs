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

namespace Business.Handlers.InterstielAdModels.Commands
{
    /// <summary>
    /// </summary>
    public class CreateInterstielAdModelCommand : IRequest<IResult>
    {
        public string ProjectId { get; set; }
        public string Name { get; set; }
        public float Version { get; set; }
        public int PlayerPercent { get; set; }
        public bool IsAdvSettingsActive { get; set; }
        public AdvStrategy[] AdvStrategies { get; set; }


        public class CreateInterstielAdModelCommandHandler : IRequestHandler<CreateInterstielAdModelCommand, IResult>
        {
            private readonly IInterstielAdModelRepository _interstielAdModelRepository;
            private readonly IMediator _mediator;

            public CreateInterstielAdModelCommandHandler(IInterstielAdModelRepository interstielAdModelRepository,
                IMediator mediator)
            {
                _interstielAdModelRepository = interstielAdModelRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateInterstielAdModelValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(LogstashLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateInterstielAdModelCommand request,
                CancellationToken cancellationToken)
            {
                var isThereInterstielAdModelRecord = _interstielAdModelRepository.Any(u =>
                    u.Name == request.Name && u.ProjectId == request.ProjectId && u.Version == request.Version);

                if (isThereInterstielAdModelRecord)
                    return new ErrorResult(Messages.AlreadyExist);

                var addedInterstielAdModel = new InterstielAdModel
                {
                    ProjectId = request.ProjectId,
                    Name = request.Name,
                    Version = request.Version,
                    PlayerPercent = request.PlayerPercent,
                    IsAdvSettingsActive = request.IsAdvSettingsActive,
                    AdvStrategies = request.AdvStrategies
                };

                await _interstielAdModelRepository.AddAsync(addedInterstielAdModel);

                return new SuccessResult(Messages.Added);
            }
        }
    }
}