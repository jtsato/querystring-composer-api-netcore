using System;
using System.Collections.Generic;
using EntryPoint.WebApi.Commons;
using Xunit;

namespace IntegrationTest.EntryPoint.WebApi.Commons;

[Collection("WebApi Collection [NoContext]")]
public sealed class OrderByHelperTest
{
    private readonly string[] _sortableFields = {"Field1", "Field3", "Field5", "Field7Asc", "Field9Desc"};

    [Trait("Category", "WebApi Collection [NoContext]")]
    [Fact(DisplayName = "Successful to sanitize empty orders by query parameters")]
    public void SuccessfulToSanitizeEmptyOrdersByQueryParameters()
    {
        // Arrange
        // Act
        // Assert
        Assert.Equal(string.Empty, OrderByHelper.Sanitize(Array.Empty<string>(), (List<string>) null));
        Assert.Equal(string.Empty, OrderByHelper.Sanitize(Array.Empty<string>(), new List<string>(0)));
        Assert.Equal(string.Empty, OrderByHelper.Sanitize(Array.Empty<string>(), new List<string> {"Id,Asc"}));
        Assert.Equal(string.Empty, OrderByHelper.Sanitize(Array.Empty<string>(), (string) null));
        Assert.Equal(string.Empty, OrderByHelper.Sanitize(Array.Empty<string>(), string.Empty));

        Assert.Equal(string.Empty, OrderByHelper.Sanitize(null, (List<string>) null));
        Assert.Equal(string.Empty, OrderByHelper.Sanitize(null, new List<string>(0)));
        Assert.Equal(string.Empty, OrderByHelper.Sanitize(null, new List<string> {"Id,Asc"}));
        Assert.Equal(string.Empty, OrderByHelper.Sanitize(null, (string) null));
        Assert.Equal(string.Empty, OrderByHelper.Sanitize(null, string.Empty));
    }

    [Trait("Category", "WebApi Collection [NoContext]")]
    [Theory(DisplayName = "Successful to filter and sanitize several types of orders")]
    [InlineData("field1,ASC,asc,AsC,aSc", "Field1:ASC")]
    [InlineData("field1,DESC,desc,DesC,dEsC", "Field1:DESC")]
    [InlineData("Asc,field1,ASC,asc,AsC,aSc", "Field1:ASC")]
    [InlineData("Desc,field1,DESC,desc,DesC,dEsC", "Field1:DESC")]
    [InlineData("field1,field3,field5", "Field1:ASC,Field3:ASC,Field5:ASC")]
    [InlineData("field1,ASC,field3,asc,field5,AsC", "Field1:ASC,Field3:ASC,Field5:ASC")]
    [InlineData("field1,DESC,field3,desc,field5,DesC", "Field1:DESC,Field3:DESC,Field5:DESC")]
    [InlineData(" field1 : desc , field2 : asc , field3 , asc ", "Field1:DESC,Field3:ASC")]
    [InlineData(" field1 : asc , field2 : desc , field3 , desc ", "Field1:ASC,Field3:DESC")]
    [InlineData("field1,Desc,field2,Asc,field3,Asc,field5,Asc", "Field1:DESC,Field3:ASC,Field5:ASC")]
    [InlineData("field1,Asc,field2,Desc,field3,Desc,field5,Desc", "Field1:ASC,Field3:DESC,Field5:DESC")]
    [InlineData("field1,DESC,field2,ASC,DESC,field3,ASC,DESC,field5", "Field1:DESC,Field3:ASC,Field5:ASC")]
    [InlineData("field1,ASC,field2,DESC,ASC,field3,DESC,ASC,field5", "Field1:ASC,Field3:DESC,Field5:ASC")]
    [InlineData("field2,Asc,field4,Desc,field6", "")]
    [InlineData("field2,Desc,field4,Asc", "")]
    [InlineData("field2,Desc", "")]
    [InlineData("field2", "")]
    [InlineData("", "")]
    [InlineData("Field7Asc,Desc", "Field7Asc:DESC")]
    [InlineData("Field9Desc,Asc", "Field9Desc:ASC")]
    [InlineData("Field9Desc", "Field9Desc:ASC")]
    public void SuccessfulToFilterAndSanitizeSeveralTypesOfOrders(string input, string expected)
    {
        // Arrange
        // Act
        // Assert
        Assert.Equal(expected, OrderByHelper.Sanitize(_sortableFields, new List<string>(input.Split(","))));
        Assert.Equal(expected, OrderByHelper.Sanitize(_sortableFields, input));
    }
}