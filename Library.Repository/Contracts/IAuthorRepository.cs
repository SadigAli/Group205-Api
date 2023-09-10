using Library.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Repository.Contracts
{
    public interface IAuthorRepository : IGenericRepository<Author>
    {
        public Task<Author> GetAuthorDetails(int id);
    }
}
