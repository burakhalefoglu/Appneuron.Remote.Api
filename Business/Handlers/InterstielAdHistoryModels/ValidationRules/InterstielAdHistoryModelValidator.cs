
using Business.Handlers.InterstielAdHistoryModels.Commands;
using FluentValidation;

namespace Business.Handlers.InterstielAdHistoryModels.ValidationRules
{

    public class CreateInterstielAdHistoryModelValidator : AbstractValidator<CreateInterstielAdHistoryModelCommand>
    {
        public CreateInterstielAdHistoryModelValidator()
        {
            RuleFor(x => x.Version).NotNull();
            RuleFor(x => x.playerPercent).NotNull();
            RuleFor(x => x.IsAdvSettingsActive).NotNull();
            RuleFor(x => x.ProjectId).NotNull();
            RuleFor(x => x.Name).NotNull();
            

        }
    }
}