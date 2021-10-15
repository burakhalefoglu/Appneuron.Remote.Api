
using Business.Handlers.RemoteOfferModels.Commands;
using FluentValidation;

namespace Business.Handlers.RemoteOfferModels.ValidationRules
{

    public class CreateRemoteOfferModelValidator : AbstractValidator<CreateRemoteOfferModelCommand>
    {
        public CreateRemoteOfferModelValidator()
        {
            RuleFor(x => x.LastPrice).NotEmpty();
            RuleFor(x => x.OfferId).NotEmpty();
            RuleFor(x => x.IsGift).NotEmpty();
            RuleFor(x => x.IsActive).NotEmpty();
            RuleFor(x => x.GiftTexture).NotEmpty();
            RuleFor(x => x.ValidityPeriod).NotEmpty();
            RuleFor(x => x.StartTime).NotEmpty();
            RuleFor(x => x.FinishTime).NotEmpty();

        }
    }
    public class UpdateRemoteOfferModelValidator : AbstractValidator<UpdateRemoteOfferModelCommand>
    {
        public UpdateRemoteOfferModelValidator()
        {
            RuleFor(x => x.LastPrice).NotEmpty();
            RuleFor(x => x.OfferId).NotEmpty();
            RuleFor(x => x.IsGift).NotEmpty();
            RuleFor(x => x.IsActive).NotEmpty();
            RuleFor(x => x.GiftTexture).NotEmpty();
            RuleFor(x => x.ValidityPeriod).NotEmpty();
            RuleFor(x => x.StartTime).NotEmpty();
            RuleFor(x => x.FinishTime).NotEmpty();

        }
    }
}