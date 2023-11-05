using System;
using Core.Commons;
using FluentValidation;

namespace Core.Domains.QueryStructures.Commands;

internal class AiSettingsCommandValidator : AbstractValidator<AiSettingsCommand>
{
    public AiSettingsCommandValidator()
    {
        RuleFor(command => command.UsagePercentage)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithMessage("ValidationUsagePercentageIsNullOrEmpty")
            .Must(ArgumentChecker.IsPercentage)
            .WithMessage("ValidationUsagePercentageIsNotPercentage");

        RuleFor(command => command.ApiKey)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("ValidationApiKeyIsNullOrEmpty")
            .When(IsAiApplicable());

        RuleFor(command => command.Model)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("ValidationModelIsNullOrEmpty")
            .When(IsAiApplicable());

        RuleFor(command => command.Temperature)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithMessage("ValidationTemperatureIsNullOrEmpty")
            .Must(ArgumentChecker.IsFloat)
            .WithMessage("ValidationTemperatureIsNotFloat")
            .When(IsAiApplicable());

        RuleFor(command => command.MaxTokens)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithMessage("ValidationMaxTokensIsNullOrEmpty")
            .Must(ArgumentChecker.IsInteger)
            .WithMessage("ValidationMaxTokensIsNotInteger")
            .Must(maxTokens => int.Parse(maxTokens) <= 4096)
            .WithMessage("ValidationMaxTokensExceedsLimit")
            .When(IsAiApplicable());

        RuleFor(command => command.PromptTemplate)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("ValidationPromptTemplateIsNullOrEmpty")
            .When(IsAiApplicable());
    }

    private static Func<AiSettingsCommand, bool> IsAiApplicable()
    {
        return command =>
        {
            if (string.IsNullOrEmpty(command.UsagePercentage)) return false;
            return int.TryParse(command.UsagePercentage, out int usagePercentage) && usagePercentage > 0;
        };
    }
}