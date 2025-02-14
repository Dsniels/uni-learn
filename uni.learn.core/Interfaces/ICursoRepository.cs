using System;
using uni.learn.core.Entities;
using uni.learn.core.Entity;

namespace uni.learn.core.Interfaces;

public interface ICursoRepository
{
    Task<IReadOnlyCollection<Curso>> GetAll();
    Task<Curso> GetByID(int id);
    Task<IReadOnlyCollection<Curso>> GetApprovedCursos();
    Task<(int likes, int dislikes)> GetVotos(int id);
    Task<int> AddVoto(Voto voto);
    Task<IReadOnlyList<CursoVisto>> GetCursoVistos(string userID);


}
