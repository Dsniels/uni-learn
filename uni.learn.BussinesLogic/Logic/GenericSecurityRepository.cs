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

   
    public async Task<IReadOnlyCollection<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

   
}
