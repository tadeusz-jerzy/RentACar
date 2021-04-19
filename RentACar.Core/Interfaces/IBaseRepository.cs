using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using RentACar.Core.Entities;

namespace RentACar.Core.Interfaces
    {
        /* 
         * TZ: Isolating from EF Core may not be strictly necessary, 
         * we still pass lambdas as conditions, 
         * but we still get better segregation of layers 
         * (Core project does not reference Infrastructure)
         * 
         * some benefits could be:
         * - this layer forces async / prevents synchronized DB access by mistake
         * - customized repositories (i.e. ICarRepository) may sometimes make sense 
         *   as containers for specialized queries (requiring deeper EF / DB skills)
         *
         */


        public interface IBaseRepository<T> where T : BaseEntity
        {
            Task<List<T>> GetAllAsync();

            Task<List<T>> GetManyAsync(Expression<Func<T, bool>> condition);
            Task<List<T>> GetManyAsync(params Expression<Func<T, bool>>[] conditions);
            Task<List<T>> GetManyAsync(List<Expression<Func<T, bool>>> conditions);

            Task<T> GetOneAsync(Expression<Func<T, bool>> condition);
            Task<T> GetOneAsync(params Expression<Func<T, bool>>[] conditions);
            Task<T> GetOneAsync(List<Expression<Func<T, bool>>> conditions);

            Task<T> FindByIdAsync(int id);
            void Add (T entity);
            void Update(T entity);
            Task<bool> DeleteAsync(int id);
        }
    }


