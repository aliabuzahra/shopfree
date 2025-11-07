using FluentAssertions;
using ShopFree.Domain.Entities;
using Xunit;

namespace ShopFree.UnitTests.Features.Products;

public class ProductTests
{
    [Fact]
    public void CreateProduct_ShouldSetCorrectProperties()
    {
        // Arrange & Act
        var product = new Product(1, "Test Product", 100.50m, "Description", "image.jpg", 10);

        // Assert
        product.StoreId.Should().Be(1);
        product.Name.Should().Be("Test Product");
        product.Price.Should().Be(100.50m);
        product.Description.Should().Be("Description");
        product.ImageUrl.Should().Be("image.jpg");
        product.Stock.Should().Be(10);
        product.IsActive.Should().BeTrue();
    }

    [Fact]
    public void UpdateProduct_ShouldUpdateProperties()
    {
        // Arrange
        var product = new Product(1, "Old Name", 50m, "Old Description", "old.jpg", 5);

        // Act
        product.Update("New Name", 75m, "New Description", "new.jpg", 15);

        // Assert
        product.Name.Should().Be("New Name");
        product.Price.Should().Be(75m);
        product.Description.Should().Be("New Description");
        product.ImageUrl.Should().Be("new.jpg");
        product.Stock.Should().Be(15);
    }

    [Fact]
    public void HasStock_WhenStockIsSufficient_ShouldReturnTrue()
    {
        // Arrange
        var product = new Product(1, "Product", 10m, null, null, 10);

        // Act & Assert
        product.HasStock(5).Should().BeTrue();
        product.HasStock(10).Should().BeTrue();
    }

    [Fact]
    public void HasStock_WhenStockIsInsufficient_ShouldReturnFalse()
    {
        // Arrange
        var product = new Product(1, "Product", 10m, null, null, 5);

        // Act & Assert
        product.HasStock(10).Should().BeFalse();
    }

    [Fact]
    public void Activate_ShouldSetIsActiveToTrue()
    {
        // Arrange
        var product = new Product(1, "Product", 10m);
        product.Deactivate();

        // Act
        product.Activate();

        // Assert
        product.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Deactivate_ShouldSetIsActiveToFalse()
    {
        // Arrange
        var product = new Product(1, "Product", 10m);

        // Act
        product.Deactivate();

        // Assert
        product.IsActive.Should().BeFalse();
    }
}

