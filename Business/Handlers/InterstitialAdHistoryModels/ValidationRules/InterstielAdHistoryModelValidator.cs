using Business.Handlers.InterstitialAdHistoryModels.Commands;
using FluentValidation;

namespace Business.Handlers.InterstielAdHistoryModels.ValidationRules
{
    public class CreateInterstielAdHistoryModelValidator : AbstractValidator<CreateInterstitialAdHistoryModelCommand>
    {
        public CreateInterstielAdHistoryModelValidator()
        {
            RuleFor(x => x.Version).NotNull();
            RuleFor(x => x.PlayerPercent).NotNull();
            RuleFor(x => x.IsAdvSettingsActive).NotNull();
            RuleFor(x => x.ProjectId).NotNull();
            RuleFor(x => x.Name).NotNull();
        }
    }
}