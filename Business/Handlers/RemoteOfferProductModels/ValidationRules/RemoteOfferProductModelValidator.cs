using Business.Handlers.RemoteOfferProductModels.Commands;
using FluentValidation;

namespace Business.Handlers.RemoteOfferProductModels.ValidationRules;

public class RemoteOfferProductModelValidator : AbstractValidator<CreateRemoteOfferProductModelCommand>
{
    public RemoteOfferProductModelValidator()
    {
        RuleFor(x => x.Count).NotNull();
        RuleFor(x => x.Version).NotNull();
        RuleFor(x => x.Image).NotNull();
        RuleFor(x => x.ImageName).NotNull();
    }
}