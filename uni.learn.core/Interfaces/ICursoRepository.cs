using System;
using uni.learn.core.Entities;
using uni.learn.core.Entity;
using uni.learn.core.Specifications;

namespace uni.learn.core.Interfaces;

public interface ICursoRepository
{
    Task<Curso> GetByID(int id);
    Task<IReadOnlyCollection<Curso>> GetApprovedCursos(ISpecification<Curso> spec);
    Task<IReadOnlyList<CursoVisto>> GetCursoVistos(string userID);
    Task<IReadOnlyCollection<Curso>> GetUnApprovedCursos();

}
