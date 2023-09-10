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
using Library.Repository.Contracts;

namespace Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthorRepository _authorRepository;

        public AuthorsController(ApplicationContext context, IMapper mapper, IAuthorRepository authorRepository)
        {
            _context = context;
            _mapper = mapper;
            _authorRepository = authorRepository;
        }

        // GET: api/Authors
        [HttpGet]
        public async Task<ActionResult> GetAuthors()
        {
            if (await _authorRepository.GetAll() == null)
            {
                return NotFound();
            }

            List<Author> authors = await _authorRepository.GetAll();
            var data = _mapper.Map<List<AuthorGetDTO>>(authors);
            return Ok(data);
        }

        // GET: api/Authors/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetAuthor(int id)
        {
            if (await _authorRepository.GetAll() == null)
            {
                return NotFound();
            }
            var author = await _authorRepository.GetAuthorDetails(id);

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

            Author author = await _authorRepository.GetById(id);
            author.Firstname = model.Firstname;
            author.Lastname = model.Lastname;
            author.UpdatedAt = DateTime.Now;

            try
            {
                await _authorRepository.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await AuthorExists(id))
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
            if (await _authorRepository.GetAll() == null)
            {
                return NotFound();
            }
            Author author = _mapper.Map<Author>(model);
            await _authorRepository.Add(author);
            await _authorRepository.Save();

            return CreatedAtAction("GetAuthor", new { id = author.Id }, model);
        }

        // DELETE: api/Authors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            if (await _authorRepository.GetAll() == null)
            {
                return NotFound();
            }
            var author = await _authorRepository.GetById(id);

            if (author == null)
            {
                return NotFound();
            }
            _authorRepository.Delete(author);
            await _authorRepository.Save();


            return NoContent();
        }

        private async Task<bool> AuthorExists(int id)
        {
            return await _authorRepository.IsExists(id);
        }
    }
}
