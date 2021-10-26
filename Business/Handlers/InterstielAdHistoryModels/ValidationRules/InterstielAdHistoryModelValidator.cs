
using Business.Handlers.InterstielAdHistoryModels.Commands;
using FluentValidation;

namespace Business.Handlers.InterstielAdHistoryModels.ValidationRules
{

    public class CreateInterstielAdHistoryModelValidator : AbstractValidator<CreateInterstielAdHistoryModelCommand>
    {
        public CreateInterstielAdHistoryModelValidator()
        {
            RuleFor(x => x.ProjectId).NotEmpty();
            RuleFor(x => x.Version).NotEmpty();
            RuleFor(x => x.playerPercent).NotEmpty();
            RuleFor(x => x.IsAdvSettingsActive).NotEmpty();

        }
    }
    public class UpdateInterstielAdHistoryModelValidator : AbstractValidator<UpdateInterstielAdHistoryModelCommand>
    {
        public UpdateInterstielAdHistoryModelValidator()
        {
            RuleFor(x => x.ProjectId).NotEmpty();
            RuleFor(x => x.Version).NotEmpty();
            RuleFor(x => x.playerPercent).NotEmpty();
            RuleFor(x => x.IsAdvSettingsActive).NotEmpty();

        }
    }
}