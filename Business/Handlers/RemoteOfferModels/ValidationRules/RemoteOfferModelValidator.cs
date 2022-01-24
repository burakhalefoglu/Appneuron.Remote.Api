﻿using Business.Handlers.RemoteOfferModels.Commands;
using FluentValidation;

namespace Business.Handlers.RemoteOfferModels.ValidationRules
{
    public class CreateRemoteOfferModelValidator : AbstractValidator<CreateRemoteOfferModelCommand>
    {
        public CreateRemoteOfferModelValidator()
        {
            RuleFor(x => x.LastPrice).NotNull();
            RuleFor(x => x.Version).NotNull();
            RuleFor(x => x.IsActive).NotNull();
            RuleFor(x => x.ValidityPeriod).NotNull();
            RuleFor(x => x.StartTime).NotNull();
            RuleFor(x => x.FinishTime).NotNull();
        }
    }

    public class UpdateRemoteOfferModelValidator : AbstractValidator<UpdateRemoteOfferModelCommand>
    {
        public UpdateRemoteOfferModelValidator()
        {
            RuleFor(x => x.ProjectId).NotNull();
            RuleFor(x => x.Version).NotNull();
            RuleFor(x => x.IsActive).NotNull();
        }
    }
}