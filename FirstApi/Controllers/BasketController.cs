using FirstApi.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace FirstApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly ApplicationContext _context; //26081922
        public BasketController(ApplicationContext context)
        {
            _context = context;
        }
        [HttpGet("/basket")]
        public IActionResult GetBasket(string user) 
        {
            if (user is null) return BadRequest(new { Message = "Istifadeci teyin olunmayib" });
            List<Basket> baskets = _context.Baskets.Where(x=>x.User == user).ToList();
            if (baskets.Count == 0) return BadRequest(new { Message = "Sebetde mehsul yoxdur" });
            return Ok(baskets);
        }

        [HttpPost("/add-to-cart/{productId}")]
        public IActionResult AddToCart(int productId,string user)
        {
            Product product = _context.Products.FirstOrDefault(x => x.Id == productId);
            if (product == null) return NotFound();
            Basket basket = _context.Baskets.
                            FirstOrDefault(x => x.ProductId == productId && x.User == user);
            if(basket != null)
            {
                basket.Count++;
            }
            else
            {
                basket = new Basket
                {
                    Count = 1,
                    ProductId = productId,
                    User = user,
                    ProductName = product.Title,
                    ProductPrice = product.Price
                };
                _context.Baskets.Add(basket);
            }
            _context.SaveChanges();
            return Ok("Mehsul sebete elave olundu");
        }
        [HttpDelete("/remove-from-cart/{productId}")]
        public IActionResult RemoveFromCart(int productId,string user) 
        {
            Basket basket = _context.Baskets.FirstOrDefault(x => x.ProductId == productId && x.User == user);
            if (basket == null) return NotFound();
            if(basket.Count > 1)
            {
                basket.Count--;
            }
            else
            {
                _context.Baskets.Remove(basket);
            }
            _context.SaveChanges();
            return Ok("Mehsul sebetden silindi");
        }
    }
}
