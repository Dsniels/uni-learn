using System;
using Microsoft.AspNetCore.Identity;

namespace uni.learn.core.Interfaces;

public interface IGenericSecurityRepository<T> where T : IdentityUser
{
    Task<IReadOnlyCollection<T>> GetAllAsync();

}
