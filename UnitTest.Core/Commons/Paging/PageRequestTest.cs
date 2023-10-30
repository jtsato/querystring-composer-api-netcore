using System;
using Core.Commons.Paging;
using Xunit;

namespace UnitTest.Core.Commons.Paging;

public sealed class PageRequestTest
{
    [Trait("Category", "Core Business tests")]
    [Fact(DisplayName = "Fail to instantiate PageRequest if PageNumber is less than ZERO")]
    public void FailToInstantiatePageRequestIfPageNumberIsLessThanZero()
    {
        // Arrange
        // Act
        // Assert
        Assert.Throws<ArgumentException>(() =>
            PageRequest.Of(-1, 0, Sort.Unsorted)
        );
    }

    [Trait("Category", "Core Business tests")]
    [Fact(DisplayName = "Fail to instantiate PageRequest if PageSize is less than ONE")]
    public void FailToInstantiatePageRequestIfPageSizeIsLessThanOne()
    {
        // Arrange
        // Act
        // Assert
        Assert.Throws<ArgumentException>(() =>
            PageRequest.Of(0, 0, Sort.Unsorted)
        );
    }

    [Trait("Category", "Core Business tests")]
    [Fact(DisplayName = "Fail to instantiate PageRequest if Sort is NULL")]
    public void FailToInstantiatePageRequestIfSortIsNull()
    {
        // Arrange
        // Act
        // Assert
        Assert.Throws<ArgumentNullException>(() =>
            PageRequest.Of(0, 1, null)
        );
    }

    [Trait("Category", "Core Business tests")]
    [Fact(DisplayName = "Successful to instantiate PageRequest if parameters are valid")]
    public void SuccessfulToInstantiatePageRequestIfParametersAreValid()
    {
        // Arrange
        // Act
        PageRequest pageRequest1 = PageRequest.Of(0, 1);
        PageRequest pageRequest2 = PageRequest.Of(1, 2, Sort.Unsorted);
        PageRequest pageRequest3 = PageRequest.Of(2, 4, Sort.By("dummyField"));

        // Assert
        Assert.Equal(0, pageRequest1.PageNumber);
        Assert.Equal(1, pageRequest1.PageSize);

        Assert.Equal(1, pageRequest2.PageNumber);
        Assert.Equal(2, pageRequest2.PageSize);
        Assert.Equal(Sort.Unsorted, pageRequest2.Sort);

        Assert.Equal(2, pageRequest3.PageNumber);
        Assert.Equal(4, pageRequest3.PageSize);
        Assert.Equal(Sort.By("dummyField"), pageRequest3.Sort);
    }
}