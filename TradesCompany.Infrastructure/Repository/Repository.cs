using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradesCompany.Application.Interfaces;
using TradesCompany.Infrastructure.Data;

namespace TradesCompany.Infrastructure.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        internal DbSet<T> _dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(object Id)
        {
            return await _dbSet.FindAsync(Id);
        }

        public async Task InsertAsync(T Entity)
        {
            await _dbSet.AddAsync(Entity);
        }

        public async Task UpdateAsync(T Entity)
        {
            _dbSet.Update(Entity);
        }

        public async Task DeleteAsync(object Id)
        {
            var entity = await _dbSet.FindAsync(Id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
