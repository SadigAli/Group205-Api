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
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly ApplicationContext _context;
        public GenericRepository(ApplicationContext context)
        {
            _context = context;
        }
        public async Task Add(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public void Delete(T entity)
        {
            entity.DeletedAt = DateTime.Now;
        }

        public async Task<List<T>> GetAll()
        {
            return await _context.Set<T>().Where(x => x.DeletedAt == null).ToListAsync();
        }

        public async Task<T> GetById(int id)
        {
            return await _context.Set<T>().Where(x=>x.DeletedAt == null).FirstOrDefaultAsync(x=>x.Id == id);
        }

        public async Task<bool> IsExists(int id)
        {
            return await _context.Set<T>().AnyAsync(x => x.Id == id);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}
