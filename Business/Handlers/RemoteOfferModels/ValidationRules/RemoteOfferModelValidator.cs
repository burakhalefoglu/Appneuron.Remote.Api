using Business.Handlers.RemoteOfferModels.Commands;
using FluentValidation;

namespace Business.Handlers.RemoteOfferModels.ValidationRules
{
    public class RemoteOfferModelValidator : AbstractValidator<CreateRemoteOfferModelCommand>
    {
        public RemoteOfferModelValidator()
        {
            RuleFor(x => x.LastPrice).NotNull();
            RuleFor(x => x.Version).NotNull();
            RuleFor(x => x.ValidityPeriod).NotNull();
            RuleFor(x => x.StartTime).NotNull();
            RuleFor(x => x.FinishTime).NotNull();
            RuleFor(x => x.GiftTexture).NotEmpty();
        }
    }

    public class UpdateRemoteOfferModelValidator : AbstractValidator<UpdateRemoteOfferModelCommand>
    {
        public UpdateRemoteOfferModelValidator()
        {
            RuleFor(x => x.ProjectId).NotNull();
            RuleFor(x => x.Version).NotNull();
        }
    }
}