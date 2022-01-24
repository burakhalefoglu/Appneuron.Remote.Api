using Business.Handlers.InterstitialAdEventModels.Commands;
using FluentValidation;

namespace Business.Handlers.InterstitialAdEventModels.ValidationRules
{
    public class CreateInterstitialAdEventModelValidator : AbstractValidator<CreateInterstitialAdEventModelCommand>
    {
        public CreateInterstitialAdEventModelValidator()
        {
            RuleFor(x => x.ClientIdList).NotEmpty();
            RuleFor(x => x.IsAdvSettingsActive).NotEmpty();
            RuleFor(x => x.AdvFrequencyStrategies).NotEmpty();
        }
    }
}