using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;

namespace SportsStore.Controllers;

public class OrderController : Controller
{
    private IOrderRepository _repository;
    private Cart cart;

    public OrderController(IOrderRepository repository, Cart cartService)
    {
        _repository = repository;
        this.cart = cartService;
    }

    public ViewResult Checkout() => View(new Order());

    [HttpPost]
    public IActionResult Checkout(Order order)
    {
        if (cart.Lines.Count == 0)
        {
            ModelState.AddModelError("", "Sorry, your cart is empty!");
        }
        if (ModelState.IsValid)
        {
            order.Lines = cart.Lines.ToArray();
            _repository.SaveOrder(order);
            cart.Clear();
            return RedirectToPage("/Completed", new { orderId = order.OrderID });
        }
        return View();
    }

}