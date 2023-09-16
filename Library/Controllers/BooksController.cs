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
using Library.Repository.Contracts;
using Microsoft.AspNetCore.Authorization;

namespace Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BooksController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IValidator<PostBookDTO> _validator;
        private readonly IBookRepository _bookRepository;
        private readonly IFileRepository _fileRepository;

        public BooksController(IMapper mapper,IValidator<PostBookDTO> validator, IBookRepository bookRepository, IFileRepository fileRepository)
        {
            _mapper = mapper;
            _validator = validator;
            _bookRepository = bookRepository;
            _fileRepository = fileRepository;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult> GetBooks()
        {
            if (await _bookRepository.GetAll() == null) return NotFound();
            List<Book> books = await _bookRepository.GetAll();
            List<GetBookDTO> data = _mapper.Map<List<GetBookDTO>>(books);
            return Ok(data);
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetBook(int id)
        {
            if (await _bookRepository.GetAll() == null) return NotFound();

            var book = await _bookRepository.GetBookDetails(id);

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
        public async Task<IActionResult> PutBook(int id, [FromForm]PostBookDTO model)
        {
            Book book = await _bookRepository.GetBookDetails(id);
            if (book is null)
            {
                return BadRequest();
            }
            if(model.File != null)
            {
                _fileRepository.DeleteFile("books", book.Image);
                book.Image = await _fileRepository.FileUpload("books", model.File);
            }
            _bookRepository.UpdateBook(model, book);
            await _bookRepository.Save();


            try
            {
                await _bookRepository.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await BookExists(id))
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
            if (await _bookRepository.GetAll() == null) return NotFound();
            var validateResult = await _validator.ValidateAsync(model);
            if (!validateResult.IsValid)
            {
                return BadRequest(validateResult.Errors.Select(x=>x.ErrorMessage).ToList());
            }
            string filePath = "";
            if(model.File != null)
            {
                filePath = await _fileRepository.FileUpload("books", model.File);
            }

            Book book = _mapper.Map<Book>(model);
            _bookRepository.AddAuthor(book, model.AuthorIds);
            book.Image = filePath;
            await _bookRepository.Add(book);
            await _bookRepository.Save();

            return CreatedAtAction("GetBook", new { id = book.Id }, book);
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            if (await _bookRepository.GetAll() == null) return NotFound();
            var book = await _bookRepository.GetById(id);
            if (book == null)
            {
                return NotFound();
            }
            _fileRepository.DeleteFile("books", book.Image);
            _bookRepository.Delete(book);
            _bookRepository.Save();

            return NoContent();
        }

        private Task<bool> BookExists(int id)
        {
            return _bookRepository.IsExists(id);
        }
    }
}
