namespace SportsStore.Models;


public class Cart
{
    public List<CartLine> Lines { get; set; } = new();

    public void AddItem(Product product, int quantity)
    {
        CartLine? line = Lines.FirstOrDefault(p => p.Product.ProductID == product.ProductID);

        if (line == null)
        {
            Lines.Add(new CartLine
            {
                Product = product,
                Quantity = quantity
            });
        }
        else
        {
            line.Quantity += quantity;
        }
    }

    public void RemoveLine(Product product) =>
        Lines.RemoveAll(x => x.Product.ProductID == product.ProductID);

    public decimal ComputeTotalValue() =>
        Lines.Sum(x => x.Product.Price * x.Quantity);

    public void Clear() => Lines.Clear();
}

public class CartLine
{
    public int Id { get; set; }
    public Product Product { get; set; } = new();
    public int Quantity { get; set; }
}