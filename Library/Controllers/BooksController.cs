using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Library.Data.Entities;
using Library.Data.DTOs.Book;
using AutoMapper;
using FluentValidation;

namespace Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;
        private readonly IValidator<PostBookDTO> _validator;
        private readonly IWebHostEnvironment _env;

        public BooksController(ApplicationContext context, IMapper mapper,IValidator<PostBookDTO> validator, IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _validator = validator;
            _env = env;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult> GetBooks()
        {
            if (_context.Books == null)
            {
                return NotFound();
            }
            List<Book> books = await _context.Books.Where(x => x.DeletedAt == null)
                .Include(x=>x.Genre)
                .Include(x=>x.BookAuthors)
                .ThenInclude(x=>x.Author)
                .ToListAsync();
            List<GetBookDTO> data = _mapper.Map<List<GetBookDTO>>(books);
            return Ok(data);
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetBook(int id)
        {
            if (_context.Books == null)
            {
                return NotFound();
            }
            var book = await _context.Books.Where(x => x.DeletedAt == null).FirstOrDefaultAsync(x => x.Id == id);

            if (book == null)
            {
                return NotFound();
            }
            GetBookDTO data = _mapper.Map<GetBookDTO>(book);
            return Ok(data);
        }

        // PUT: api/Books/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, Book book)
        {
            if (id != book.Id)
            {
                return BadRequest();
            }

            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
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

        // POST: api/Books
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook([FromForm]PostBookDTO model)
        {
            if (_context.Books == null)
            {
                return Problem("Entity set 'ApplicationContext.Books'  is null.");
            }
            var validateResult = await _validator.ValidateAsync(model);
            if (!validateResult.IsValid)
            {
                return BadRequest(validateResult.Errors.Select(x=>x.ErrorMessage).ToList());
            }
            string filePath = "";
            if(model.File != null)
            {
                string folderPath = Path.Combine(_env.WebRootPath, "uploads", "books"); // C:\Users\sadig\OneDrive\Desktop\Group205-Api\Library\wwwroot\uploads\books
                filePath = Guid.NewGuid() + "_" + model.File.FileName;
                string fullPath = Path.Combine(folderPath, filePath);
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                using(FileStream stream = new FileStream(fullPath, FileMode.Create))
                {
                    await model.File.CopyToAsync(stream);
                }
            }

            Book book = _mapper.Map<Book>(model);
            book.BookAuthors = new List<BookAuthor>();
            foreach (var authorId in model.AuthorIds)
            {
                book.BookAuthors.Add(new BookAuthor
                {
                    AuthorId = authorId,
                });
            }
            book.Image = filePath;
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBook", new { id = book.Id }, book);
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            if (_context.Books == null)
            {
                return NotFound();
            }
            var book = await _context.Books.Where(x => x.DeletedAt == null).FirstOrDefaultAsync(x => x.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            book.DeletedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookExists(int id)
        {
            return (_context.Books?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
