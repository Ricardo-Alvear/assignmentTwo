using COMP2139___assignment2.Data;
using COMP2139___assignment2.ViewModels.Cart;
using comp2147.data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace COMP2139___assignment2.Controllers;

[Authorize]
public class CartController : Controller
{
  
  private readonly ApplicationDbContext _db;

  public CartController(ApplicationDbContext db)
  {
    _db = db;
  }
  
  [HttpPost]
  public async Task<IActionResult> Add([FromBody] AddToCartVM model)
  {
    var ticket = await _db.TicketTypes.FindAsync(model.TicketTypeId);
    if (ticket == null) return NotFound();
    
    int remaining = ticket.Quantity - ticket.QuantitySold;
    if (model.Quantity > remaining) model.Quantity = remaining;
    
    // Add to session/cart
    var cartCount = 1;
    var total = ticket.Price * model.Quantity;

    return Json(new { count = cartCount, total, lowStock = remaining <= 7, remaining });
  }

  [HttpPost]
  public async Task<IActionResult> ChangeQuantity([FromBody] ChangeQuantityVM model)
  {
    var ticket = await _db.TicketTypes.FirstOrDefaultAsync(t => t.Id == model.TicketTypeId);
    if (ticket == null)
    {
      return NotFound();
    }

    int remaining = ticket.Quantity - ticket.QuantitySold;

    if (model.Quantity > remaining)
    {
      model.Quantity = remaining;
    }

    decimal total = ticket.Price * model.Quantity;
    return Json(new
    {
      count = model.Quantity,
      total,
      lowStock = remaining <= 7,
      remaining
    });
  }
}