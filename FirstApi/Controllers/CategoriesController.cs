using FirstApi.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FirstApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ApplicationContext _context;
        public CategoriesController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<Category> categories = _context.Categories.Include(x=>x.Products).ToList();
            if (categories.Count == 0) return BadRequest(new { Message = "No data found"});
            return Ok(categories);
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            try
            {
                _context.Categories.Add(category);
                _context.SaveChanges(); 
            }
            catch (Exception ex)
            {
                return BadRequest(new {Message = ex.Message });
            }
            return CreatedAtAction(nameof(Index),category);
        }

        [HttpGet("{id}")]
        public IActionResult Details(int id) 
        {
            Category category = _context.Categories.Find(id);
            if(category == null) return NotFound();
            return Ok(category);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id,Category model)
        {
            Category category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();
            category.Name = model.Name;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Category category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
