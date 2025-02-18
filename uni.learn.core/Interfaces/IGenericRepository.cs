using System;
using uni.learn.core.Entities;
using uni.learn.core.Entity;
using uni.learn.core.Specifications;

namespace uni.learn.core.Interfaces;

public interface IGenericRepository<T> where T : Base
{
    Task<T> GetByID(int id);
    Task<IReadOnlyCollection<T>> GetAllAsync();
    Task<IReadOnlyCollection<T>> GetAllWithSpec(ISpecification<T> spec);
    Task<int> CountAsync();
    Task<int> Add(T entity);
    Task<int> Update(T entity);
    Task<int> DeleteEntity(T entity);


}
