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
using Library.Repository.Contracts;

namespace Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IGenreRepository _genreRepository;

        public GenresController(IMapper mapper,IGenreRepository genreRepository)
        {
            _mapper = mapper;
            _genreRepository = genreRepository;
        }

        // GET: api/Genres
        [HttpGet]
        public async Task<ActionResult> GetGenres()
        {
          if (await _genreRepository.GetAll() == null)
          {
              return NotFound();
          }
            List<Genre> genres = await _genreRepository.GetAll();
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
            if (await _genreRepository.GetAll() == null)
            {
                return NotFound();
            }
            var genre = await _genreRepository.GetById(id);

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
            Genre genre = await _genreRepository.GetById(id);
            //genre.Name = model.Name;
            //genre.UpdatedAt = DateTime.Now;
            _genreRepository.Update(genre);
            try
            {
                await _genreRepository.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await GenreExists(id))
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
            if (await _genreRepository.GetAll() == null)
            {
                return NotFound();
            }
            #region Manual Mapper
            //Genre genre = new Genre
            //{
            //    Name = model.Name,
            //};
            #endregion

            Genre genre = _mapper.Map<Genre>(model);
            await _genreRepository.Add(genre);
            await _genreRepository.Save();
            return CreatedAtAction("GetGenre", new { id = genre.Id }, model);
        }

        // DELETE: api/Genres/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenre(int id)
        {
            if (await _genreRepository.GetAll() == null)
            {
                return NotFound();
            }
            var genre = await _genreRepository.GetById(id);
            if (genre == null)
            {
                return NotFound();
            }

            _genreRepository.Delete(genre);
            await _genreRepository.Save();

            return NoContent();
        }

        private async Task<bool> GenreExists(int id)
        {
            return await _genreRepository.IsExists(id);
        }
    }
}
