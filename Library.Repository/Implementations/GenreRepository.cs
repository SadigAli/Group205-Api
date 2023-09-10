using Library.Data.Entities;
using Library.Repository.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Repository.Implementations
{
    public class GenreRepository : GenericRepository<Genre>,IGenreRepository
    {
        public GenreRepository(ApplicationContext context) : base(context)
        {
        }

        public async Task<Genre> GetGenreDetails(int id)
        {
            return await _context.Genres
                        .Where(x => x.DeletedAt == null)
                        .Include(x => x.Books)
                        .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
