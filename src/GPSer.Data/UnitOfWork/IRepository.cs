﻿using Ardalis.Specification;
using GPSer.Model;

namespace GPSer.Data.UnitOfWork
{
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        TEntity GetById(int id);
        TEntity GetById(string id);
        TEntity GetById(Guid id);
        TEntity FirstOrDefault(ISpecification<TEntity> spec);
        IQueryable<TEntity> ListAll();
        TEntity Add(TEntity entity);
        IReadOnlyCollection<TEntity> Add(IReadOnlyCollection<TEntity> entities);
        void Update(TEntity entity);
        void Update(IReadOnlyCollection<TEntity> entities);
        void Delete(TEntity entity);
        void Delete(IReadOnlyCollection<TEntity> entities);
        int Count();

        // Async methods

        Task<TEntity> GetByIdAsync(int id);
        Task<TEntity> GetByIdAsync(string id);
        Task<TEntity> GetByIdAsync(Guid id);
        Task<TEntity> FirstOrDefaultAsync();
        Task<TEntity> FirstOrDefaultAsync(CancellationToken cancellationToken);
        Task<TEntity> FirstOrDefaultAsync(ISpecification<TEntity> spec);
        Task<IReadOnlyList<TEntity>> ListAllAsync();
        Task<IReadOnlyList<TEntity>> ListAsync(ISpecification<TEntity> spec);
        Task<TEntity> AddAsync(TEntity entity);
        Task<int> AddAsync(IReadOnlyCollection<TEntity> entities);
        Task UpdateAsync(TEntity entity);
        Task UpdateAsync(IReadOnlyCollection<TEntity> entities);
        Task DeleteAsync(TEntity entity);
        Task DeleteAsync(IReadOnlyCollection<TEntity> entities);
        Task<int> CountAsync();
    }
}