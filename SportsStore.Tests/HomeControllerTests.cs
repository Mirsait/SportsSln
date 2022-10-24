using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using SportsStore.ViewModels;

namespace SportsStore.Tests;

public class HomeControllerTests
{
    [Fact]
    public void Can_Use_Repository()
    {
        // Arrange
        var mock = new Mock<IStoreRepository>();
        var products = new Product[] {
            new Product{Id = 1, Name = "P1"},
            new Product{Id = 2, Name = "P2"},
        };
        mock.Setup(m => m.Products).Returns(products.AsQueryable<Product>());
        var controller = new HomeController(mock.Object);

        // Act
        ProductListViewModel result = controller.Index(null)?.ViewData.Model
            as ProductListViewModel
            ?? new();

        // Assert
        Product[] prodArray = result.Products.ToArray();
        Assert.True(prodArray.Length == 2);
        Assert.Equal("P1", prodArray[0].Name);
        Assert.Equal("P2", prodArray[1].Name);
    }

    [Fact]
    public void Can_Paginate()
    {
        // Arrange
        var mock = new Mock<IStoreRepository>();
        var products = new Product[] {
            new Product{Id = 1, Name = "P1"},
            new Product{Id = 2, Name = "P2"},
            new Product{Id = 3, Name = "P3"},
            new Product{Id = 4, Name = "P4"},
            new Product{Id = 5, Name = "P5"},
        };
        mock.Setup(m => m.Products).Returns(products.AsQueryable<Product>());
        var controller = new HomeController(mock.Object);
        controller.PageSize = 3;

        // Act
        ProductListViewModel result = controller.Index(null, 2)?.ViewData.Model
            as ProductListViewModel
            ?? new();

        // Assert
        Product[] prodArray = result.Products.ToArray();
        Assert.True(prodArray.Length == 2);
        Assert.Equal("P4", prodArray[0].Name);
        Assert.Equal("P5", prodArray[1].Name);
    }

    [Fact]
    public void Can_Send_Pagination_View_Model()
    {
        // Arrange
        var mock = new Mock<IStoreRepository>();
        var products = new Product[] {
            new Product{Id = 1, Name = "P1"},
            new Product{Id = 2, Name = "P2"},
            new Product{Id = 3, Name = "P3"},
            new Product{Id = 4, Name = "P4"},
            new Product{Id = 5, Name = "P5"},
        };
        mock.Setup(m => m.Products).Returns(products.AsQueryable<Product>());
        var controller = new HomeController(mock.Object) { PageSize = 3 };

        // Act
        ProductListViewModel result = controller.Index(null, 2)?.ViewData.Model
            as ProductListViewModel
            ?? new();

        // Assert
        PagingInfo pageInfo = result.PagingInfo;
        Assert.Equal(2, pageInfo.CurrentPage);
        Assert.Equal(3, pageInfo.ItemsPerPage);
        Assert.Equal(5, pageInfo.TotalItems);
        Assert.Equal(2, pageInfo.TotalPages);
    }

    [Fact]
    public void Can_Filter_Products()
    {
        // arrange
        var mock = new Mock<IStoreRepository>();
        var products = new Product[] {
            new Product{Id = 1, Name = "P1", Category = "Cat1"},
            new Product{Id = 2, Name = "P2", Category = "Cat2"},
            new Product{Id = 3, Name = "P3", Category = "Cat1"},
            new Product{Id = 4, Name = "P4", Category = "Cat2"},
            new Product{Id = 5, Name = "P5", Category = "Cat3"},
        };
        mock.Setup(m => m.Products).Returns(products.AsQueryable<Product>());
        var controller = new HomeController(mock.Object);
        controller.PageSize = 3;

        // action
        Product[] result = (controller.Index("Cat2", 1)?.ViewData.Model
            as ProductListViewModel ?? new()).Products.ToArray();

        // asserts
        Assert.Equal(2, result.Length);
        Assert.True(result[0].Name == "P2" && result[0].Category == "Cat2");
        Assert.True(result[1].Name == "P4" && result[1].Category == "Cat2");
    }
}