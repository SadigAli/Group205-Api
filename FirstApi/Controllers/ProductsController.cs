using FirstApi.DTOs.Product;
using FirstApi.Entities;
using FirstApi.Services;
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
        private readonly FileService _fileService;
        public ProductsController(ApplicationContext context, IWebHostEnvironment env,FileService fileService)
        {
            _context = context;
            _env = env;
            _fileService = fileService;
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

                product.Image = await _fileService.FileUpload(_env.WebRootPath, "products", model.File) ;
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
        public async Task<IActionResult> Update(int id,[FromForm]ProductPostDTO model)
        {
            Product product = _context.Products.Find(id);
            if (product is null) return NotFound();
            product.Title= model.Title;
            product.Price = model.Price;
            product.Description = model.Description;
            product.Count = model.Count;
            if(model.File != null)
            {
                if (model.File.Length / 1024 > 500)
                    return BadRequest(new { Message = "File's length must be less than 500kb" });
                if (!model.File.ContentType.Contains("image")) // image/png,image/jpeg,image/svg
                    return BadRequest(new { Message = "File's format must be an image" });
                _fileService.FileDelete(_env.WebRootPath, product.Image, "products");
                product.Image = await _fileService.FileUpload(_env.WebRootPath, "products", model.File);
            }
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Product product = _context.Products.Find(id);
            if (product is null) return NotFound();
            _fileService.FileDelete(_env.WebRootPath, product.Image, "products");
            _context.Products.Remove(product);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
