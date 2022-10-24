using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Moq;
using SportsStore.Components;
using SportsStore.Models;

namespace SportsStore.Tests;

public class NavigationMenuViewComponentTests
{
    [Fact]
    public void Can_Select_Categories()
    {
        var mock = new Mock<IStoreRepository>();
        var products = new Product[] {
            new Product{Id = 1, Name = "P1", Category = "Cat1"},
            new Product{Id = 2, Name = "P2", Category = "Cat2"},
            new Product{Id = 3, Name = "P3", Category = "Cat3"},
            new Product{Id = 4, Name = "P4", Category = "Cat3"},
        };
        mock.Setup(m => m.Products).Returns(products.AsQueryable<Product>());

        var target = new NavigationMenuViewComponent(mock.Object);

        // act
        string[] results = ((IEnumerable<string>?)(target.Invoke()
        as ViewViewComponentResult)?.ViewData?.Model ?? Enumerable.Empty<string>()).ToArray();

        // asserts
        Assert.True(Enumerable.SequenceEqual(new string[] { "Cat1", "Cat2", "Cat3" }, results));
    }

    [Fact]
    public void Indicates_Selected_Category()
    {
        // arrange
        string categoryToSelect = "Sky-Watcher";
        var mock = new Mock<IStoreRepository>();
        var products = new Product[] {
            new Product{Id = 1, Name = "P1", Category = "Meade"},
            new Product{Id = 2, Name = "P2", Category = "Sky-Watcher"},
        };
        mock.Setup(m => m.Products).Returns(products.AsQueryable<Product>());

        var target = new NavigationMenuViewComponent(mock.Object);
        target.ViewComponentContext = new ViewComponentContext
        {
            ViewContext = new ViewContext
            {
                RouteData = new Microsoft.AspNetCore.Routing.RouteData()
            }
        };
        target.RouteData.Values["category"] = categoryToSelect;

        // action
        string? result = (string?)(target.Invoke()
            as ViewViewComponentResult)?.ViewData?["SelectedCategory"];

        // assert
        Assert.Equal(categoryToSelect, result);
    }

}
