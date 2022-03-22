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

public class DeleteAdvStrategyCommand : IRequest<IResult>
{
    public string Name { get; set; }
    public float Count { get; set; }
    public long ProjectId { get; set; }

    public class DeleteAdvStrategyCommandHandler : IRequestHandler<DeleteAdvStrategyCommand, IResult>
    {
        private readonly IAdvStrategyRepository _advStrategyRepository;

        public DeleteAdvStrategyCommandHandler(IAdvStrategyRepository advStrategyRepository)
        {
            _advStrategyRepository = advStrategyRepository;
        }

        [ValidationAspect(typeof(AdvStrategyValidator), Priority = 2)]
        [CacheRemoveAspect("Get")]
        [LogAspect(typeof(ConsoleLogger))]
        [SecuredOperation(Priority = 1)]
        public async Task<IResult> Handle(DeleteAdvStrategyCommand request,
            CancellationToken cancellationToken)
        {
            await _advStrategyRepository.DeleteAsync(new AdvStrategy
            {
                StrategyValue = request.Count,
                Name = request.Name,
                ProjectId = request.ProjectId
            });

            return new SuccessResult(Messages.Deleted);
        }
    }
}