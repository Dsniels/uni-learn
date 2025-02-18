using System;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using uni.learn.BussinesLogic.Data;
using uni.learn.core.Entities;
using uni.learn.core.Interfaces;

namespace uni.learn.BussinesLogic.Logic;

public class VotosRepository : IVotosRepostory
{
    private readonly MainDbContext _context;

    public VotosRepository(MainDbContext context)
    {
        _context = context;
    }


    public async Task<(int likes, int dislikes)> GetVotos(int id)
    {
        int likes = await _context.Voto.CountAsync(v => v.CursoId == id && v.Like == true);
        int dislikes = await _context.Voto.CountAsync(v => v.CursoId == id && v.Like == false);
        return (likes, dislikes);
    }

    public async Task<bool> UserLikeCurso(string userId, int cursoId)
    {
        var res = await _context.Voto.FirstAsync(v => v.CursoId == cursoId && v.UsuarioId == userId);
        return res != null ? true : false;
    }


    public async Task<Voto> GetVotoAsync(string userId, int cursoId)
    {
        return await _context.Voto.FirstOrDefaultAsync(v => v.CursoId == cursoId && v.UsuarioId == userId);

    }
}
