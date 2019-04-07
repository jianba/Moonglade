﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Moonglade.Data.Infrastructure
{
    public class DbContextRepository<T> : IRepository<T>, IAsyncRepository<T> where T : class
    {
        protected readonly MoongladeDbContext DbContext;

        public DbContextRepository(MoongladeDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public T Get(object key)
        {
            return DbContext.Set<T>().Find(key);
        }

        public IReadOnlyList<T> Get(bool asNoTracking = true)
        {
            return asNoTracking ?
                DbContext.Set<T>().AsNoTracking().ToList() :
                DbContext.Set<T>().ToList();
        }

        public IReadOnlyList<T> Get(ISpecification<T> spec, bool asNoTracking = true)
        {
            return asNoTracking ?
                ApplySpecification(spec).AsNoTracking().ToList() :
                ApplySpecification(spec).ToList();
        }

        public T Add(T entity)
        {
            DbContext.Set<T>().Add(entity);
            DbContext.SaveChanges();

            return entity;
        }

        public int Update(T entity)
        {
            DbContext.Entry(entity).State = EntityState.Modified;
            return DbContext.SaveChanges();
        }

        public int Delete(T entity)
        {
            DbContext.Set<T>().Remove(entity);
            return DbContext.SaveChanges();
        }

        public int Count(ISpecification<T> spec)
        {
            return ApplySpecification(spec).Count();
        }

        public virtual Task<T> GetAsync(object key)
        {
            return DbContext.Set<T>().FindAsync(key);
        }

        public async Task<IReadOnlyList<T>> GetAsync(bool asNoTracking = true)
        {
            if (asNoTracking)
            {
                return await DbContext.Set<T>().AsNoTracking().ToListAsync();
            }
            return await DbContext.Set<T>().ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAsync(ISpecification<T> spec, bool asNoTracking = true)
        {
            if (asNoTracking)
            {
                return await ApplySpecification(spec).AsNoTracking().ToListAsync();
            }
            return await ApplySpecification(spec).ToListAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            DbContext.Set<T>().Add(entity);
            await DbContext.SaveChangesAsync();

            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            DbContext.Entry(entity).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            DbContext.Set<T>().Remove(entity);
            await DbContext.SaveChangesAsync();
        }

        public Task<int> CountAsync(ISpecification<T> spec)
        {
            return ApplySpecification(spec).CountAsync();
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(DbContext.Set<T>().AsQueryable(), spec);
        }
    }
}