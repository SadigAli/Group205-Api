using FirstApi.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FirstApi.Controllers
{
    [Route("api/[controller]")] // localhost:7281/api/products
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly List<Product> products;
        public ProductsController()
        {
            products = new List<Product>
            {
                new Product { Id = 1, Title = "Test 1",Description ="Test 1",Price = 15},
                new Product { Id = 2, Title = "Test 2",Description ="Test 2",Price = 5},
                new Product { Id = 3, Title = "Test 3",Description ="Test 3",Price = 25},
                new Product { Id = 4, Title = "Test 4",Description ="Test 4",Price = 55},
            };
        }

        [HttpGet]
        public IEnumerable<Product> GetProducts()
        {
            return products;
        }

        [HttpGet("{id}")]
        public Product GetProduct(int id) 
        {
            return products.FirstOrDefault(x => x.Id == id);
        }

        [HttpPost]

        public IEnumerable<Product> AddProduct(Product product)
        {
            products.Add(product);
            return products;
        }
        [HttpPut("{id}")]
        public Product EditProduct(int id,Product model) 
        {
            Product product = products.FirstOrDefault(x => x.Id == id);
            product.Title = model.Title;
            product.Description = model.Description;
            product.Price = model.Price;
            return product;
        }

        [HttpDelete("{id}")]
        public IEnumerable<Product> DeleteProduct(int id)
        {
            Product product = products.FirstOrDefault(x => x.Id==id);
            products.Remove(product);
            return products;
        }
    }
}
