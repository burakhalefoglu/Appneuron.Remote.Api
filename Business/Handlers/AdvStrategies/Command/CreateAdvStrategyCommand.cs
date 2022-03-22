using Business.BusinessAspects;
using Business.Constants;
using Business.Handlers.AdvStrategies.ValidationRules;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;

namespace Business.Handlers.AdvStrategies.Command;

public class CreateAdvStrategyCommand : IRequest<IResult>
{
    public string Name { get; set; }
    public float Count { get; set; }
    public long ProjectId { get; set; }
    public string StrategyName { get; set; }
    public string Version { get; set; }

    public class CreateAdvStrategyCommandHandler : IRequestHandler<CreateAdvStrategyCommand, IResult>
    {
        private readonly IAdvStrategyRepository _advStrategyRepository;

        public CreateAdvStrategyCommandHandler(IAdvStrategyRepository advStrategyRepository)
        {
            _advStrategyRepository = advStrategyRepository;
        }

        [ValidationAspect(typeof(AdvStrategyValidator), Priority = 2)]
        [CacheRemoveAspect("Get")]
        [LogAspect(typeof(ConsoleLogger))]
        [SecuredOperation(Priority = 1)]
        public async Task<IResult> Handle(CreateAdvStrategyCommand request,
            CancellationToken cancellationToken)
        {
            await _advStrategyRepository.AddAsync(new AdvStrategy
            {
                StrategyValue = request.Count,
                Name = request.Name,
                ProjectId = request.ProjectId,
                Version = request.Version,
                StrategyName = request.StrategyName
            });

            return new SuccessResult(Messages.Added);
        }
    }
}