using FluentValidation;

namespace Core.Domains.Clients.Commands;

internal sealed class RegisterClientCommandValidator : AbstractValidator<RegisterClientCommand>
{
    public RegisterClientCommandValidator()
    {
        RuleFor(command => command.Uid)
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
    }
}