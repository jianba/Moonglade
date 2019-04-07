﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moonglade.Data.Infrastructure
{
    public interface IAsyncRepository<T> where T : class
    {
        Task<T> GetAsync(object key);

        Task<IReadOnlyList<T>> GetAsync(bool asNoTracking = true);

        Task<IReadOnlyList<T>> GetAsync(ISpecification<T> spec, bool asNoTracking = true);

        Task<T> AddAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);

        Task<int> CountAsync(ISpecification<T> spec);
    }
}