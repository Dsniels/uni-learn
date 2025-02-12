using System;
using uni.learn.core.Entity;

namespace uni.learn.core.Interfaces;

public interface IGenericRepository<T> where T : Base
{
    Task<T> GetByID(int id);
    Task<IReadOnlyCollection<T>> GetAllAsync();
    Task<int> CountAsync();
    Task<int> Add(T entity);
    Task<int> Update(T entity);
    Task<int> DeleteEntity(T entity);

    void AddEntity(T entity);

}
