
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;

namespace SportsStore.Components;

public class NavigationMenuViewComponent : ViewComponent
{
    private IStoreRepository _repository;

    public NavigationMenuViewComponent(IStoreRepository repository)
    {
        _repository = repository;
    }

    public IViewComponentResult Invoke()
    {
        ViewBag.SelectedCategory = RouteData?.Values["category"];
        var catNames = _repository.Products
            .Select(x => x.Category)
            .Distinct()
            .OrderBy(x => x);
        return View(catNames);
    }
}