using System.Collections.Generic;
using FluentValidation;

namespace Core.Domains.QueryStructures.Commands;

public sealed class AddQueryStructureItemCommand
{
    private static readonly AddQueryStructureItemCommandValidator Validator = new AddQueryStructureItemCommandValidator();

    public int QueryStructureId { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public bool IsCountable { get; init; }
    public bool WaitForConfirmationWords { get; init; }
    public IList<string> ConfirmationWords { get; init; }
    public IList<string> RevocationWords { get; init; }

    public AddQueryStructureItemCommand
    (
        int queryStructureId,
        string name,
        string description,
        bool isCountable,
        bool waitForConfirmationWords,
        IList<string> confirmationWords,
        IList<string> revocationWords
    )
    {
        QueryStructureId = queryStructureId;
        Name = name;
        Description = description;
        IsCountable = isCountable;
        WaitForConfirmationWords = waitForConfirmationWords;
        ConfirmationWords = confirmationWords;
        RevocationWords = revocationWords;
        Validator.ValidateAndThrow(this);
    }
}