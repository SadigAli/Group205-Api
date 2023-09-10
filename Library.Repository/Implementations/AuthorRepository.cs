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
    public class AuthorRepository : GenericRepository<Author>, IAuthorRepository
    {
        public AuthorRepository(ApplicationContext context) : base(context)
        {
        }

        public async Task<Author> GetAuthorDetails(int id)
        {
            return await _context.Authors
                   .Where(x => x.DeletedAt == null)
                   .Include(x => x.BookAuthors)
                   .ThenInclude(x => x.Book)
                   .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
