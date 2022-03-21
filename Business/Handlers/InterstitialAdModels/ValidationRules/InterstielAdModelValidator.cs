using Business.Handlers.InterstitialAdModels.Commands;
using FluentValidation;

namespace Business.Handlers.InterstitialAdModels.ValidationRules
{
    public class CreateInterstielAdModelValidator : AbstractValidator<CreateInterstitialAdModelCommand>
    {
        public CreateInterstielAdModelValidator()
        {
            RuleFor(x => x.Version).NotNull();
            RuleFor(x => x.PlayerPercent).NotNull();
            RuleFor(x => x.AdvStrategies).NotNull();
            RuleFor(x => x.ProjectId).NotNull();
        }
    }

    public class UpdateInterstielAdModelValidator : AbstractValidator<UpdateInterstitialAdModelCommand>
    {
        public UpdateInterstielAdModelValidator()
        {
            RuleFor(x => x.Version).NotNull();
            RuleFor(x => x.PlayerPercent).NotNull();
            RuleFor(x => x.ProjectId).NotNull();
        }
    }
}