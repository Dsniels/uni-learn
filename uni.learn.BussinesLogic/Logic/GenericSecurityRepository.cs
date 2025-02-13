using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using uni.learn.BussinesLogic.Context;
using uni.learn.core.Interfaces;

namespace uni.learn.BussinesLogic.Logic;

public class GenericSecurityRepository<T> : IGenericSecurityRepository<T> where T : IdentityUser
{
    private readonly SecurityDbContext _context;
    public GenericSecurityRepository(SecurityDbContext context){
        _context = context;
    }

    public Task<int> Add(T entity)
    {
        throw new NotImplementedException();
    }

    public async Task<IReadOnlyCollection<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public Task<T> GetByID(string Id)
    {
        throw new NotImplementedException();
    }

    public Task<int> Update(T entity)
    {
        throw new NotImplementedException();
    }
}
