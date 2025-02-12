using System;
using Microsoft.AspNetCore.Identity;

namespace uni.learn.core.Interfaces;

public interface IGenericSecurityRepository<T> where T : IdentityUser
{
    Task<T> GetByID(string Id);
    Task<IReadOnlyCollection<T>> GetAllAsync();
    Task<int> Add(T entity);
    Task<int> Update(T entity);

}
