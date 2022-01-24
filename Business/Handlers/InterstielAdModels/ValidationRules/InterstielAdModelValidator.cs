using Business.Handlers.InterstielAdModels.Commands;
using FluentValidation;

namespace Business.Handlers.InterstielAdModels.ValidationRules
{
    public class CreateInterstielAdModelValidator : AbstractValidator<CreateInterstielAdModelCommand>
    {
        public CreateInterstielAdModelValidator()
        {
            RuleFor(x => x.Version).NotNull();
            RuleFor(x => x.PlayerPercent).NotNull();
            RuleFor(x => x.IsAdvSettingsActive).NotNull();
            RuleFor(x => x.AdvStrategies).NotNull();
            RuleFor(x => x.ProjectId).NotNull();
        }
    }

    public class UpdateInterstielAdModelValidator : AbstractValidator<UpdateInterstielAdModelCommand>
    {
        public UpdateInterstielAdModelValidator()
        {
            RuleFor(x => x.Version).NotNull();
            RuleFor(x => x.PlayerPercent).NotNull();
            RuleFor(x => x.IsAdvSettingsActive).NotNull();
            RuleFor(x => x.ProjectId).NotNull();
        }
    }
}