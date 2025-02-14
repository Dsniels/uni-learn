using System;
using Microsoft.EntityFrameworkCore;
using uni.learn.BussinesLogic.Data;
using uni.learn.core.Entities;
using uni.learn.core.Entity;
using uni.learn.core.Interfaces;

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

    public async Task<IReadOnlyCollection<Curso>> GetAll()
    {
        return await _context.Curso.Include(c => c.Temas).ToListAsync();
    }

    public async Task<IReadOnlyCollection<Curso>> GetApprovedCursos()
    {
        return await _context.Curso.Include(t => t.Temas)
                                    .Where(c => c.Aprobado == true)
                                    .ToListAsync();
    }

    public async Task<Curso> GetByID(int id)
    {
        return await _context.Curso.Include(t => t.Temas)
                                    .FirstOrDefaultAsync(c => c.Id == id);

    }

    public async Task<IReadOnlyList<CursoVisto>> GetCursoVistos(string userID)
    {
        return await _context.CursoVistos.Where(c => c.UsuarioId == userID)
                                        .Include(t => t.Curso)
                                        .ToListAsync();
    }

    public async Task<(int likes, int dislikes)> GetVotos(int id)
    {
        int likes = await _context.Voto.CountAsync(v => v.CursoId == id && v.Like == true);
        int dislikes = await _context.Voto.CountAsync(v => v.CursoId == id && v.Like == false);
        return (likes, dislikes);
    }


}
