using Library.Data.DTOs.Book;
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
    public class BookRepository : GenericRepository<Book>, IBookRepository
    {
        public BookRepository(ApplicationContext context) : base(context)
        {
        }

        public void AddAuthor(Book book, List<int> authorIds)
        {
            book.BookAuthors = new List<BookAuthor>();
            foreach (var authorId in authorIds)
            {
                book.BookAuthors.Add(new BookAuthor
                {
                    AuthorId = authorId,
                });
            }
        }

        public async Task<Book> GetBookDetails(int id)
        {
            return await _context.Books
                        .Include(x => x.Genre)
                        .Include(x => x.BookAuthors)
                        .ThenInclude(x => x.Author)
                        .FirstOrDefaultAsync(x => x.Id == id);
        }

        public void UpdateBook(PostBookDTO model,Book book)
        {
            book.Page = (int)model.Page;
            book.Name = model.Name;
            book.UpdatedAt = DateTime.Now;
            book.GenreId = (int)model.GenreId;

            // 3,5 - old
            List<int> oldIds = book.BookAuthors.Select(x=>x.AuthorId).ToList();
            // 1,3,6 - new model.AuthorIds
            // 5 - removed
            List<int> removeIds = oldIds.FindAll(x => !model.AuthorIds.Contains(x));
            // 1,6 - adds
            List<int> addIds = model.AuthorIds.FindAll(x=>!oldIds.Contains(x));

            book.BookAuthors.RemoveAll(x => removeIds.Contains(x.AuthorId));

            foreach (int id in addIds) 
            { 
                book.BookAuthors.Add(new BookAuthor { AuthorId = id});
            }
        }

    }
}
