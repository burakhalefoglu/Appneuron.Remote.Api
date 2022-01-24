using Business.Handlers.RemoteOfferEventModels.Commands;
using FluentValidation;

namespace Business.Handlers.RemoteOfferEventModels.ValidationRules
{
    public class CreateRemoteOfferEventModelValidator : AbstractValidator<CreateRemoteOfferEventModelCommand>
    {
        public CreateRemoteOfferEventModelValidator()
        {
            RuleFor(x => x.ClientIdList).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.IsActive).NotEmpty();
            RuleFor(x => x.FirstPrice).NotEmpty();
            RuleFor(x => x.LastPrice).NotEmpty();
            RuleFor(x => x.Version).NotEmpty();
            RuleFor(x => x.PlayerPercent).NotEmpty();
            RuleFor(x => x.IsGift).NotEmpty();
            RuleFor(x => x.ValidityPeriod).NotEmpty();
            RuleFor(x => x.StartTime).NotEmpty();
            RuleFor(x => x.FinishTime).NotEmpty();
        }
    }
}