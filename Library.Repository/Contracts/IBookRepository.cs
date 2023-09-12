using Library.Data.DTOs.Book;
using Library.Data.Entities;
using Library.Repository.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Repository.Contracts
{
    public interface IBookRepository : IGenericRepository<Book>
    {
        public Task<Book> GetBookDetails(int id);

        public void AddAuthor(Book book,List<int> authorIds);

        public void UpdateBook(PostBookDTO model, Book book);
    }
}
