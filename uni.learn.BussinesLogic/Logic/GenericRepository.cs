using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using uni.learn.BussinesLogic.Context.Data;
using uni.learn.BussinesLogic.Data;
using uni.learn.core.Entity;
using uni.learn.core.Interfaces;
using uni.learn.core.Specifications;

namespace uni.learn.BussinesLogic.Logic;

public class GenericRepository<T> : IGenericRepository<T> where T : Base
{
    private readonly MainDbContext _context;
    public GenericRepository(MainDbContext context)
    {
        _context = context;
    }
    public async Task<int> Add(T entity)
    {
        _context.Set<T>().Add(entity);
        return await _context.SaveChangesAsync();
    }

    public async Task<int> AddEntity(List<T> entity)
    {
        _context.Set<T>().AddRange(entity);

        return  await _context.SaveChangesAsync();

    }

    public Task<int> CountAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<int> DeleteEntity(T entity)
    {

        _context.Set<T>().Remove(entity);
        return await _context.SaveChangesAsync();

    }

    public async Task<IReadOnlyCollection<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
        
    }

    public async Task<IReadOnlyCollection<T>> GetAllWithSpec(ISpecification<T> spec)
    {
      return await ApplySpecification(spec).ToListAsync(); 
    }



    private IQueryable<T> ApplySpecification(ISpecification<T> spec){
        return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), spec);
    }

    public async Task<T> GetByID(int id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public async Task<int> Update(T entity)
    {
        _context.Set<T>().Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
        return await _context.SaveChangesAsync();
    }
}
