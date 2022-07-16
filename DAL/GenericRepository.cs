using Microsoft.EntityFrameworkCore;
using NewsApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NewsApp.DAL
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected AppDbContext _context;
        internal DbSet<T> dbSet;


        public GenericRepository(AppDbContext context)
        {
            this._context = context;
            this.dbSet = context.Set<T>();

        }

        public async Task<bool> Add(T entity)
        {

            await dbSet.AddAsync(entity);
            return true;


        }

        public virtual async Task<IEnumerable<T>> GetAll()
        {
            return await dbSet.ToListAsync();
        }




        public virtual async Task<T> GetById(string id)
        {
            return await dbSet.FindAsync(id);
        }

        public virtual Task<bool> Update(T entity)
        {

            _context.Entry(entity).State = EntityState.Modified;
            return Task.FromResult(true);


        }

        public async Task<IEnumerable<T>> FindByQuery(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            else
            {
                return await query.ToListAsync();
            }
        }

        public Task<bool> Delete(T entity)
        {

            dbSet.Remove(entity);
            return Task.FromResult(true);

        }
    }
}
