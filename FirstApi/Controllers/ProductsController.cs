using FirstApi.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FirstApi.Controllers
{
    [Route("api/[controller]")] // localhost:7281/api/products
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationContext _context;
        public ProductsController(ApplicationContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            List<Product> products = _context.Products.Include(x=>x.Category).ToList();
            if (products.Count == 0) return BadRequest(new { Message = "Data not found"});
            return Ok(products);
        }

        [HttpGet("{id}")]

        public IActionResult Details(int id)
        {
            Product product = _context.Products.Find(id);
            if(product is null) return NotFound();
            return Ok(product);
        }

        [HttpPost]

        public IActionResult Create(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
            return Created("/api/products", _context.Products.ToList());
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id,Product model)
        {
            Product product = _context.Products.Find(id);
            if (product is null) return NotFound();
            product.Title= model.Title;
            product.Price = model.Price;
            product.Description = model.Description;
            product.Count = model.Count;
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Product product = _context.Products.Find(id);
            if (product is null) return NotFound();
            _context.Products.Remove(product);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
