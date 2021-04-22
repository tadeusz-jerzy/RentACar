using Microsoft.EntityFrameworkCore;
using RentACar.Core.Entities;
using RentACar.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;



namespace RentACar.Infrastructure
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly DbContext _context;
        protected readonly DbSet<T> _entities;
        protected virtual IQueryable<T> IncludingRelated => _entities;

        public BaseRepository(DbContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await IncludingRelated.ToListAsync();
        }

        private IQueryable<T> QueryForConditions(
            List<Expression<Func<T, bool>>> conditions, 
            bool tracking = true)
        {
            IQueryable<T> q = tracking 
                ? IncludingRelated.AsQueryable()
                : IncludingRelated.AsNoTracking().AsQueryable() ;

            foreach (var cond in conditions)
                q = q.Where(cond);
            
            return q.AsQueryable();
        }


        public async Task<List<T>> GetManyAsync(Expression<Func<T, bool>> condition)
        {
            var q = await GetManyAsync(condition, c => true);
            return q;
        }

        public async Task<List<T>> GetManyAsync(params Expression<Func<T, bool>>[] conditions)
        {
            var q = QueryForConditions(conditions.ToList());
            var result = await q.ToListAsync();
            return result;
        }

        public async Task<List<T>> GetManyAsync(List<Expression<Func<T, bool>>> conditions)
        {
            var q = QueryForConditions(conditions);
            var result = await q.ToListAsync();
            return result;
        }

        public async Task<T> FindByIdAsync(int id)
        {
            return await GetOneAsync(e => e.Id == id);
        }

        public void Add(T entity)
        {
            _entities.Add(entity);
        }

        public void Update(T entity)
        {
            _entities.Update(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            T entity = await FindByIdAsync(id);
            if (entity == null)
                return false;

            _entities.Remove(entity);
            return true;
        }

        public async Task<T> GetOneAsync(Expression<Func<T, bool>> condition)
        {
            return await GetOneAsync(condition, e => true);
        }

        public async Task<T> GetOneAsync(params Expression<Func<T, bool>>[] conditions)
        {
            return await GetOneAsync(conditions.ToList());
        }

        public async Task<T> GetOneAsync(List<Expression<Func<T, bool>>> conditions)
        {
            var q = QueryForConditions(conditions);
            var result = await q.FirstOrDefaultAsync();
            return result;
        }
    }
}
