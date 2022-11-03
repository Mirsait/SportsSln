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
            new Product{ProductID = 1, Name = "P1"},
            new Product{ProductID = 2, Name = "P2"},
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
            new Product{ProductID = 1, Name = "P1"},
            new Product{ProductID = 2, Name = "P2"},
            new Product{ProductID = 3, Name = "P3"},
            new Product{ProductID = 4, Name = "P4"},
            new Product{ProductID = 5, Name = "P5"},
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
            new Product{ProductID = 1, Name = "P1"},
            new Product{ProductID = 2, Name = "P2"},
            new Product{ProductID = 3, Name = "P3"},
            new Product{ProductID = 4, Name = "P4"},
            new Product{ProductID = 5, Name = "P5"},
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
            new Product{ProductID = 1, Name = "P1", Category = "Cat1"},
            new Product{ProductID = 2, Name = "P2", Category = "Cat2"},
            new Product{ProductID = 3, Name = "P3", Category = "Cat1"},
            new Product{ProductID = 4, Name = "P4", Category = "Cat2"},
            new Product{ProductID = 5, Name = "P5", Category = "Cat3"},
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

    [Fact]
    public void Generate_Category_Specific_Product_Count()
    {
        // arrange
        var mock = new Mock<IStoreRepository>();
        var products = new Product[] {
            new Product{ProductID = 1, Name = "P1", Category = "Cat1"},
            new Product{ProductID = 2, Name = "P2", Category = "Cat1"},
            new Product{ProductID = 3, Name = "P3", Category = "Cat3"},
            new Product{ProductID = 4, Name = "P4", Category = "Cat1"},
            new Product{ProductID = 5, Name = "P5", Category = "Cat2"},
        };
        mock.Setup(m => m.Products).Returns(products.AsQueryable<Product>());
        var target = new HomeController(mock.Object);
        target.PageSize = 3;

        Func<ViewResult, ProductListViewModel?> GetModel = result =>
            result?.ViewData?.Model as ProductListViewModel;

        // Action
        int? res1 = GetModel(target.Index("Cat1"))?.PagingInfo.TotalItems;
        int? res2 = GetModel(target.Index("Cat2"))?.PagingInfo.TotalItems;
        int? res3 = GetModel(target.Index("Cat3"))?.PagingInfo.TotalItems;
        int? resAll = GetModel(target.Index(null))?.PagingInfo.TotalItems;
        // Assert
        Assert.Equal(3, res1);
        Assert.Equal(1, res2);
        Assert.Equal(1, res3);
        Assert.Equal(5, resAll);
    }
}