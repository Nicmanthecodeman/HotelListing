using HotelListing.Data;
using HotelListing.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HotelListing.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DatabaseContext _db;
        internal readonly DbSet<T> _dbSet;

        public GenericRepository(
            DatabaseContext db)
        {
            _db = db;
            _dbSet = _db.Set<T>();
        }

        public async Task Delete(int id)
        {
            T entity = await _dbSet.FindAsync(id);
            _dbSet.Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public async Task<T> Get(
            Expression<Func<T, bool>> filter = null, 
            List<string> includedProperties = null)
        {
            IQueryable<T> query = _dbSet;

            if (includedProperties != null)
            {
                foreach (string includedProperty in includedProperties)
                {
                    query = query.Include(includedProperty);
                }
            }            

            return await query.AsNoTracking()
                .FirstOrDefaultAsync(filter);
        }

        public async Task<IList<T>> GetAll(
            Expression<Func<T, bool>> filter = null, 
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, 
            List<string> includedProperties = null)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includedProperties != null)
            {
                foreach (string includedProperty in includedProperties)
                {
                    query = query.Include(includedProperty);
                }
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await query.AsNoTracking()
                .ToListAsync();
        }

        public async Task Insert(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task InsertRange(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public void Update(T entity)
        {
            _dbSet.Attach(entity);
            _db.Entry(entity).State = EntityState.Modified;
        }
    }
}
