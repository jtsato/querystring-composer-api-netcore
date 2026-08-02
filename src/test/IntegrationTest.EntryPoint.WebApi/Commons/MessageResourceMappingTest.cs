using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Core.Commons.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTest.EntryPoint.WebApi.Commons;

[Collection("WebApi Collection [NoContext]")]
public sealed class MessageResourceMappingTest(ITestOutputHelper outputHelper, CoreMessageKeysFixture coreMessageKeysFixture) : IClassFixture<CoreMessageKeysFixture>
{
    private static readonly string WebApiProjectFolder = GetProjectsByCsprojFile.Projects["EntryPoint.WebApi"];
    
    private const string DefaultMessageResourcePath = "Messages.resx";
    private const string EnUsMessageResourcePath = "Messages.en-US.resx";
    private const string PtBrMessageResourcePath = "Messages.pt-BR.resx";

    [Trait("Category", "WebApi Collection [NoContext]")]
    [Theory(DisplayName = "Successful to validate messages mapped in message resources ")]
    [InlineData(DefaultMessageResourcePath)]
    [InlineData(EnUsMessageResourcePath)]
    [InlineData(PtBrMessageResourcePath)]
    public void SuccessfulToValidateMessagesMappedInMessageResources(string messageResource)
    {
        // Arrange
        string messageResourcePath = $"{WebApiProjectFolder}/Resources/{messageResource}";
        List<string> messageResourcesKeys = GetMessageResourcesKeysByFilePath(messageResourcePath);

        // Act 
        IEnumerable<string> coreProjectMessageKeys = coreMessageKeysFixture.GetCoreProjectMessageKeys();
        IEnumerable<string> messageKeysNotMappedInResourceFile =
            GetMessagesKeysNotMappedInResourceFile(messageResource, coreProjectMessageKeys, messageResourcesKeys);

        // Assert
        Assert.Empty(messageKeysNotMappedInResourceFile);
    }

    [ExcludeFromCodeCoverage]
    private IEnumerable<string> GetMessagesKeysNotMappedInResourceFile
    (
        string messageResource,
        IEnumerable<string> coreProjectMessageKeys,
        ICollection<string> messageResourcesKeys
    )
    {
        List<string> messageKeysNotMappedInResourceFile = [];
        IEnumerable<string> messageKeys = coreProjectMessageKeys.Where(messageKey => !messageResourcesKeys.Contains(messageKey));

        foreach (string messageKey in messageKeys)
        {
            messageKeysNotMappedInResourceFile.Add(messageKey);
            outputHelper.WriteLine(
                $"The message key \"{messageKey}\" was not found in resource file {messageResource}");
        }

        return messageKeysNotMappedInResourceFile;
    }

    private static List<string> GetMessageResourcesKeysByFilePath(string messageResourcePath)
    {
        return
        [
            .. from line in File.ReadLines(messageResourcePath)
            where line.Contains("<data name=\"")
            select line.SubstringAfter("<data name=\"").SubstringBefore("\" xml:space")
        ];
    }
}