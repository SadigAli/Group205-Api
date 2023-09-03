using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Library.Data.Entities;
using AutoMapper;
using Library.Data.DTOs.Author;

namespace Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;

        public AuthorsController(ApplicationContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Authors
        [HttpGet]
        public async Task<ActionResult> GetAuthors()
        {
          if (_context.Authors == null)
          {
              return NotFound();
          }

            List<Author> authors = await _context.Authors
                    .Where(x=>x.DeletedAt == null)
                    .ToListAsync();
            var data = _mapper.Map<List<AuthorGetDTO>>(authors);
            return Ok(data);
        }

        // GET: api/Authors/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetAuthor(int id)
        {
          if (_context.Authors == null)
          {
              return NotFound();
          }
            var author = await _context.Authors
                .Where(x=>x.DeletedAt == null)
                .FirstOrDefaultAsync(x=>x.Id == id);

            if (author == null)
            {
                return NotFound();
            }

            var data = _mapper.Map<AuthorGetDTO>(author);
            return Ok(data);
        }

        // PUT: api/Authors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthor(int id, AuthorGetDTO model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            //Author author = _mapper.Map<Author>(model);
            Author author = await _context.Authors.FirstOrDefaultAsync(x => x.Id == id);
            author.Firstname = model.Firstname;
            author.Lastname = model.Lastname;
            author.UpdatedAt = DateTime.Now;

            _context.Entry(author).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuthorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Authors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult> PostAuthor(AuthorPostDTO model)
        {
          if (_context.Authors == null)
          {
              return Problem("Entity set 'ApplicationContext.Authors'  is null.");
          }
            Author author = _mapper.Map<Author>(model);
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAuthor", new { id = author.Id }, model);
        }

        // DELETE: api/Authors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            if (_context.Authors == null)
            {
                return NotFound();
            }
            var author = await _context.Authors
                    .Where(x=>x.DeletedAt == null)
                    .FirstOrDefaultAsync(x => x.Id == id);
                
                    ;
            if (author == null)
            {
                return NotFound();
            }
            author.DeletedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AuthorExists(int id)
        {
            return (_context.Authors?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
