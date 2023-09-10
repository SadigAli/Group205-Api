using Library.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Repository.Contracts
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        public Task Save();
        public Task Add(T entity);
        public void Update(T entity);
        public void Delete(T entity);
        public Task<List<T>> GetAll();
        public Task<T> GetById(int id);

        public Task<bool> IsExists(int id);
    }
}
