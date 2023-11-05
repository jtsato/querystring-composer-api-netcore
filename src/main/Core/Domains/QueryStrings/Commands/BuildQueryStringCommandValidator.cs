using FluentValidation;

namespace Core.Domains.QueryStrings.Commands;

internal sealed class BuildQueryStringCommandValidator : AbstractValidator<BuildQueryStringCommand>
{
    public BuildQueryStringCommandValidator()
    {
        RuleFor(command => command.ClientUid)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("ValidationClientUidIsNullOrEmpty");

        RuleFor(command => command.QueryName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("ValidationQueryNameIsNullOrEmpty");

        RuleFor(command => command.SearchTerms)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("ValidationSearchTermsIsNullOrEmpty");
    }
}