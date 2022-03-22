using Business.Handlers.AdvStrategies.Command;
using FluentValidation;

namespace Business.Handlers.AdvStrategies.ValidationRules;

public class AdvStrategyValidator : AbstractValidator<CreateAdvStrategyCommand>
{
    public AdvStrategyValidator()
    {
        RuleFor(x => x.Count).NotNull();
        RuleFor(x => x.Name).NotNull();
    }
}