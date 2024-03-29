﻿using System.Linq;
using System.Threading.Tasks;

namespace Restaurant.Data.Contracts
{
    public interface IRepository<TEntity>
        where TEntity : class
    {
        IQueryable<TEntity> All();

        Task AddAsync(TEntity entity);

        void Delete(TEntity entity);

        void Update(TEntity entity);

        Task<int> SaveChangesAsync();
    }
}
