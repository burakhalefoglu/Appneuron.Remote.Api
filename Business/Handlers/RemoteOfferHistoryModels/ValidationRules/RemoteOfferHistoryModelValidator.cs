
using Business.Handlers.RemoteOfferHistoryModels.Commands;
using FluentValidation;

namespace Business.Handlers.RemoteOfferHistoryModels.ValidationRules
{

    public class CreateRemoteOfferHistoryModelValidator : AbstractValidator<CreateRemoteOfferHistoryModelCommand>
    {
        public CreateRemoteOfferHistoryModelValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.IsActive).NotNull();
            RuleFor(x => x.FirstPrice).NotEmpty();
            RuleFor(x => x.LastPrice).NotEmpty();
            RuleFor(x => x.Version).NotEmpty();
            RuleFor(x => x.PlayerPercent).NotEmpty();
            RuleFor(x => x.IsGift).NotNull();
            RuleFor(x => x.ValidityPeriod).NotEmpty();
            RuleFor(x => x.StartTime).NotEmpty();
            RuleFor(x => x.FinishTime).NotEmpty();

        }
    }
}