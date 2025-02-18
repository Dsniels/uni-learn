using System;
using Microsoft.EntityFrameworkCore;
using uni.learn.BussinesLogic.Context.Data;
using uni.learn.BussinesLogic.Data;
using uni.learn.core.Entities;
using uni.learn.core.Entity;
using uni.learn.core.Interfaces;
using uni.learn.core.Specifications;

namespace uni.learn.BussinesLogic.Logic;

public class CursoRepository : ICursoRepository
{
    private readonly MainDbContext _context;
    public CursoRepository(MainDbContext context)
    {
        _context = context;
    }

    public async Task<int> AddVoto(Voto voto)
    {
        _context.Voto.Add(voto);
        return await _context.SaveChangesAsync();

    }


    private IQueryable<Curso> ApplySpecifications(ISpecification<Curso> spec)
    {
        return SpecificationEvaluator<Curso>.GetQuery(_context.Curso.AsQueryable(), spec);
    }

    public async Task<IReadOnlyCollection<Curso>> GetApprovedCursos(ISpecification<Curso> spec)
    {
        return await ApplySpecifications(spec).Where(C => C.Aprobado == true).ToListAsync();
    }

    public async Task<Curso> GetByID(int id)
    {
        return await _context.Curso.Include(t => t.Temas)
                                    .FirstOrDefaultAsync(c => c.Id == id);

    }

    public async Task<int> CountAsync(ISpecification<Curso> spec)
    {
        return await ApplySpecifications(spec).CountAsync();
    }

    public async Task<IReadOnlyList<CursoVisto>> GetCursoVistos(string userID)
    {
        return await _context.CursoVistos.Where(c => c.UsuarioId == userID)
                                        .Include(t => t.Curso)
                                        .ToListAsync();
    }


    public async Task<IReadOnlyCollection<Curso>> GetUnApprovedCursos()
    {
        return await _context.Curso.Where(c => c.Aprobado == false).ToListAsync();

    }





}
