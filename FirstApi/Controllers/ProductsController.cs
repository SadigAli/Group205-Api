using FirstApi.DTOs.Product;
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
        private readonly IWebHostEnvironment _env;
        public ProductsController(ApplicationContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;

        }
        [HttpGet]
        public IActionResult Index()
        {
            List<Product> products = _context.Products
                            .Include(x=>x.Category)
                            .Include(x=>x.ProductColors)
                            .ThenInclude(x=>x.Color)
                            .ToList();

            if (products.Count == 0) return BadRequest(new { Message = "Data not found"});
            var data = new List<ProductGetDTO>();
            foreach (var product in products)
            {
                data.Add(new ProductGetDTO
                {
                    Id = product.Id,
                    Title = product.Title,
                    Count = product.Count,
                    Price = product.Price,
                    Category = product.Category.Name,
                    Colors = product.ProductColors.Select(x=>x.Color.Name).ToList(),
                    Image = product.Image != null 
                                ? $"{Request.Scheme}://{Request.Host.Value}/uploads/products/{product.Image}"
                                : "", 
                });
            }
            return Ok(data);
        }

        [HttpGet("{id}")]

        public IActionResult Details(int id)
        {
            Product product = _context.Products.Find(id);
            if(product is null) return NotFound();
            return Ok(product);
        }

        [HttpPost]

        public async Task<IActionResult> Create([FromForm]ProductPostDTO model)
        {
            Product product = new Product()
            {
                CategoryId = model.CategoryId,
                Title = model.Title,
                Price = model.Price,
                Description = model.Description,
                Count = model.Count,
            };

            if (model.File != null)
            {
                if (model.File.Length / 1024 > 500)
                    return BadRequest(new { Message = "File's length must be less than 500kb" });
                if (!model.File.ContentType.Contains("image")) // image/png,image/jpeg,image/svg
                    return BadRequest(new { Message = "File's format must be an image" });
                string filePath = Path.Combine(_env.WebRootPath, "uploads", "products");
                string fileName = Guid.NewGuid().ToString() + "_" + model.File.FileName;
                string fullPath = Path.Combine(filePath, fileName);
                // C:/geksgjh/FirstApi/wwwroot/uploads/products/filename
                using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                {
                    await model.File.CopyToAsync(stream);
                }
                product.Image = fileName;
            }
            _context.Products.Add(product);
            product.ProductColors = new List<ProductColor>();
            foreach (int colorId in model.ColorIds)
            {
                product.ProductColors.Add(new ProductColor
                {
                    ColorId = colorId
                });
            }
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
