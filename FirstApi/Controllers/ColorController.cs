using FirstApi.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FirstApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColorController : ControllerBase
    {
        private readonly ApplicationContext _context;
        public ColorController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<Color> colors = _context.Colors.ToList();
            if (colors.Count == 0) return BadRequest(new { Message = "No data found"});
            return Ok(colors);
        }

        [HttpPost]
        public IActionResult Create(Color color)
        {
            try
            {
                _context.Colors.Add(color);
                _context.SaveChanges(); 
            }
            catch (Exception ex)
            {
                return BadRequest(new {Message = ex.Message });
            }
            return CreatedAtAction(nameof(Index),color);
        }

        [HttpGet("{id}")]
        public IActionResult Details(int id) 
        {
            Color color = _context.Colors.Find(id);
            if(color == null) return NotFound();
            return Ok(color);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id,Color model)
        {
            Color color = await _context.Colors.FindAsync(id);
            if (color == null) return NotFound();
            color.Name = model.Name;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Color color = await _context.Colors.FindAsync(id);
            if (color == null) return NotFound();
            _context.Colors.Remove(color);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
