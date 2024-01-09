using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using GPSer.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GPSer.Data.UnitOfWork;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
{
    private readonly GPSerDbContext dbContext;
    private readonly ILogger<Repository<TEntity>> logger;
    private readonly ISpecificationEvaluator specificationEvaluator;

    //public Repository(GPSerDbContext dbContext, ILogger<Repository<TEntity>> logger)
    //{
    //    this.dbContext = dbContext;
    //    this.logger = logger;
    //}

    public Repository(GPSerDbContext dbContext, ILogger<Repository<TEntity>> logger)
    {
        this.dbContext = dbContext;
        this.logger = logger;
        specificationEvaluator = new SpecificationEvaluator();
    }

    TEntity? IRepository<TEntity>.GetById(int id) => dbContext.Set<TEntity>().Find(id);

    TEntity IRepository<TEntity>.GetById(string id) => dbContext.Set<TEntity>().Find(id);

    TEntity IRepository<TEntity>.GetById(Guid id) => dbContext.Set<TEntity>().Find(id);

    TEntity IRepository<TEntity>.FirstOrDefault(ISpecification<TEntity> spec) => ApplySpecification(spec).FirstOrDefault();

    IQueryable<TEntity> IRepository<TEntity>.ListAll() => dbContext.Set<TEntity>();

    TEntity IRepository<TEntity>.Add(TEntity entity)
    {
        dbContext.Set<TEntity>().Add(entity);
        dbContext.SaveChanges();

        return entity;
    }

    IReadOnlyCollection<TEntity> IRepository<TEntity>.Add(IReadOnlyCollection<TEntity> entities)
    {
        dbContext.Set<TEntity>().AddRange(entities);
        dbContext.SaveChanges();

        return entities;
    }

    void IRepository<TEntity>.Update(TEntity entity)
    {
        dbContext.Entry(entity).State = EntityState.Modified;
        dbContext.SaveChanges();
    }

    void IRepository<TEntity>.Update(IReadOnlyCollection<TEntity> entities)
    {
        dbContext.UpdateRange(entities);
        dbContext.SaveChanges();
    }

    void IRepository<TEntity>.Delete(TEntity entity)
    {
        dbContext.Set<TEntity>().Remove(entity);
        dbContext.SaveChanges();
    }

    void IRepository<TEntity>.Delete(IReadOnlyCollection<TEntity> entities)
    {
        dbContext.Set<TEntity>().RemoveRange(entities);
        dbContext.SaveChanges();
    }

    int IRepository<TEntity>.Count() => dbContext.Set<TEntity>().Count();

    async Task<TEntity> IRepository<TEntity>.GetByIdAsync(int id) => await dbContext.Set<TEntity>().FindAsync(id);

    async Task<TEntity> IRepository<TEntity>.GetByIdAsync(string id) => await dbContext.Set<TEntity>().FindAsync(id);

    async Task<TEntity> IRepository<TEntity>.GetByIdAsync(Guid id) => await dbContext.Set<TEntity>().FindAsync(id);

    async Task<TEntity> IRepository<TEntity>.FirstOrDefaultAsync() => await dbContext.Set<TEntity>().FirstOrDefaultAsync();

    async Task<TEntity> IRepository<TEntity>.FirstOrDefaultAsync(CancellationToken cancellationToken) => await dbContext.Set<TEntity>().FirstOrDefaultAsync(cancellationToken);

    async Task<TEntity> IRepository<TEntity>.FirstOrDefaultAsync(ISpecification<TEntity> spec) => await ApplySpecification(spec).FirstOrDefaultAsync();

    async Task<IReadOnlyList<TEntity>> IRepository<TEntity>.ListAllAsync() => await dbContext.Set<TEntity>().ToListAsync();

    async Task<IReadOnlyList<TEntity>> IRepository<TEntity>.ListAsync(ISpecification<TEntity> spec) => await ApplySpecification(spec).ToListAsync();

    async Task<TEntity> IRepository<TEntity>.AddAsync(TEntity entity)
    {
        await dbContext.Set<TEntity>().AddAsync(entity);
        await dbContext.SaveChangesAsync();

        return entity;
    }

    async Task<int> IRepository<TEntity>.AddAsync(IReadOnlyCollection<TEntity> entities)
    {
        await dbContext.Set<TEntity>().AddRangeAsync(entities);
        return await dbContext.SaveChangesAsync();
    }

    async Task IRepository<TEntity>.UpdateAsync(TEntity entity)
    {
        dbContext.Entry(entity).State = EntityState.Modified;
        await dbContext.SaveChangesAsync();
    }

    async Task IRepository<TEntity>.UpdateAsync(IReadOnlyCollection<TEntity> entities)
    {
        dbContext.UpdateRange(entities);
        await dbContext.SaveChangesAsync();
    }

    async Task IRepository<TEntity>.DeleteAsync(TEntity entity)
    {
        dbContext.Set<TEntity>().Remove(entity);
        await dbContext.SaveChangesAsync();
    }

    async Task IRepository<TEntity>.DeleteAsync(IReadOnlyCollection<TEntity> entities)
    {
        dbContext.Set<TEntity>().RemoveRange(entities);
        await dbContext.SaveChangesAsync();
    }

    async Task<int> IRepository<TEntity>.CountAsync() => await dbContext.Set<TEntity>().CountAsync();

    protected IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> specification)
    {
        return specificationEvaluator.GetQuery(dbContext.Set<TEntity>().AsQueryable(), specification);
    }

    protected IQueryable<TResult> ApplySpecification<TResult>(ISpecification<TEntity, TResult> specification)
    {
        if (specification is null)
        {
            throw new ArgumentNullException(nameof(specification), "Specification is required");
        }

        if (specification.Selector is null)
        {
            throw new SelectorNotFoundException();
        }

        return specificationEvaluator.GetQuery(dbContext.Set<TEntity>().AsQueryable(), specification);
    }
}