using System;
using uni.learn.core.Entities;

namespace uni.learn.core.Interfaces;

public interface ITemasRepository
{

    Task<IReadOnlyList<Temas>> GetTemas ();

 }
