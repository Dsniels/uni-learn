using System;
using uni.learn.core.Entities;

namespace uni.learn.core.Interfaces;

public interface IVotosRepostory
{



    Task<(int likes, int dislikes)> GetVotos(int id);
    Task<bool> UserLikeCurso(string userId, int cursoId);
    Task<Voto> GetVotoAsync(string userId, int cursoId);

}
