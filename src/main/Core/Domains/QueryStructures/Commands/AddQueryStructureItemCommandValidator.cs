using FluentValidation;

namespace Core.Domains.QueryStructures.Commands;

internal class AddQueryStructureItemCommandValidator : AbstractValidator<AddQueryStructureItemCommand>
{
    public AddQueryStructureItemCommandValidator()
    {
        RuleFor(command => command.QueryStructureId)
            .Cascade(CascadeMode.Stop)
            .GreaterThan(0)
            .WithMessage("ValidationQueryStructureIdIsLessThanZero");
        
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
