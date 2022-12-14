using SportsStore.Models;

namespace SportsStore.ViewModels;

public class ProductListViewModel
{
    public IEnumerable<Product> Products { get; set; } = Enumerable.Empty<Product>();

    public PagingInfo PagingInfo { get; set; } = new();

    public string? CurrentCategory { get; set; }

}