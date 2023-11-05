using FluentValidation;

namespace Core.Domains.QueryStructures.Commands;

internal class RegisterQueryStructureCommandValidator : AbstractValidator<RegisterQueryStructureCommand>
{
    private static readonly AiSettingsCommandValidator AiSettingsCommandValidator = new AiSettingsCommandValidator();
    
    public RegisterQueryStructureCommandValidator()
    {
        RuleFor(command => command.ClientUid)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("ValidationClientUidIsNullOrEmpty");

        RuleFor(command => command.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("ValidationNameIsNullOrEmpty");

        RuleFor(command => command.Description)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("ValidationDescriptionIsNullOrEmpty");

        RuleFor(command => command.AiSettings)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithMessage("ValidationAiSettingsIsNull")
            .SetValidator(AiSettingsCommandValidator);
    }
}