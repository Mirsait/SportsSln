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
        ProductListViewModel result = controller.Index()?.ViewData.Model
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
        ProductListViewModel result = controller.Index(2)?.ViewData.Model
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
        ProductListViewModel result = controller.Index(2)?.ViewData.Model
            as ProductListViewModel
            ?? new();

        // Assert
        PagingInfo pageInfo = result.PagingInfo;
        Assert.Equal(2, pageInfo.CurrentPage);
        Assert.Equal(3, pageInfo.ItemsPerPage);
        Assert.Equal(5, pageInfo.TotalItems);
        Assert.Equal(2, pageInfo.TotalPages);
    }
}