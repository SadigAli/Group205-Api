using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Library.Data.Entities;
using Library.Data.DTOs.Genre;
using AutoMapper;

namespace Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;

        public GenresController(ApplicationContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Genres
        [HttpGet]
        public async Task<ActionResult> GetGenres()
        {
          if (_context.Genres == null)
          {
              return NotFound();
          }
            List<Genre> genres = await _context.Genres.Where(x=>x.DeletedAt == null).ToListAsync();
            #region Manual Mapper
            //List<GenreGetDTO> data = new List<GenreGetDTO>();
            //foreach (var genre in genres)
            //{
            //    data.Add(new GenreGetDTO
            //    {
            //        Id = genre.Id,
            //        Name = genre.Name,
            //    });
            //}
            #endregion
            var data = _mapper.Map<List<GenreGetDTO>>(genres);
            return Ok(data);
        }

        // GET: api/Genres/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetGenre(int id)
        {
          if (_context.Genres == null)
          {
              return NotFound();
          }
            var genre = await _context.Genres.Where(x=>x.DeletedAt == null).FirstOrDefaultAsync(x=>x.Id ==id);

            if (genre == null)
            {
                return NotFound();
            }

            #region Manual Mapper
            //GenreGetDTO data = new GenreGetDTO
            //{
            //    Id = genre.Id,
            //    Name = genre.Name,
            //};
            #endregion
            var data = _mapper.Map<GenreGetDTO>(genre);

            return Ok(data);
        }

        // PUT: api/Genres/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGenre(int id, GenreGetDTO model)
        {
            Genre genre = await _context.Genres.Where(x=>x.DeletedAt==null).FirstOrDefaultAsync(x=>x.Id ==id);
            genre.Name = model.Name;
            genre.UpdatedAt = DateTime.Now;
            _context.Entry(genre).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GenreExists(id))
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

        // POST: api/Genres
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult> PostGenre(GenrePostDTO model)
        {
          if (_context.Genres == null)
          {
              return Problem("Entity set 'ApplicationContext.Genres'  is null.");
          }
            #region Manual Mapper
            //Genre genre = new Genre
            //{
            //    Name = model.Name,
            //};
            #endregion

            Genre genre = _mapper.Map<Genre>(model);
            _context.Genres.Add(genre);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGenre", new { id = genre.Id }, model);
        }

        // DELETE: api/Genres/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenre(int id)
        {
            if (_context.Genres == null)
            {
                return NotFound();
            }
            var genre = await _context.Genres.Where(x=>x.DeletedAt == null).FirstOrDefaultAsync(x=>x.Id == id);
            if (genre == null)
            {
                return NotFound();
            }

            genre.DeletedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GenreExists(int id)
        {
            return (_context.Genres?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
