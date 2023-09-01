using FirstApi.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FirstApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationContext _context;
        public OrdersController(ApplicationContext context)
        {
            _context = context;
        }
        [HttpGet("/orders")]
        public IActionResult GetOrders(string user)
        {
            if (user == null) return BadRequest(new { Message = "Enter username" });
            List<Order> orders = _context.Orders
                .Include(x=>x.OrderProducts)
                .ThenInclude(x=>x.Product)
                .Where(x=>x.User == user).ToList();
            return Ok(orders);
        }

        [HttpPost("/order")]
        public IActionResult Order(string user)
        {
            if (user == null) return BadRequest(new { Message = "Enter username" });
            List<Basket> baskets = _context.Baskets.Where(x => x.User == user).ToList();
            if (baskets.Count == 0) return BadRequest(new { Message = "Your basket is empty" });
            Order order = new Order
            {
                Date = DateTime.Now,
                User = user,
                Total = baskets.Sum(x=>x.Count * x.ProductPrice),
                OrderStatus = Status.Pending
            };
            foreach (var basket in baskets)
            {
                order.OrderProducts.Add(new OrderProduct 
                { 
                    ProductId = basket.ProductId,
                    ProductName = basket.ProductName,
                    ProductPrice = basket.ProductPrice,
                    Count = basket.Count
                });
                _context.Baskets.Remove(basket);
            }

            _context.Orders.Add(order);
            _context.SaveChanges();

            return Ok(new { Message = "Your order has been created" });
        }

        [HttpPatch("/order/change-status/{id}")]
        public IActionResult ChangeStatus(int id,int status) 
        {
            Order order = _context.Orders.Find(id);
            if (order == null) return NotFound();
            order.OrderStatus = (Status)status;
            _context.SaveChanges();
            return Ok(new { Message = "Your order's has been successfully changed" });

        }
    }
}
