using System;
using uni.learn.core.Entity;

namespace uni.learn.core.Entities;

public class CursoVisto : Base
{

    public int CursoId { get; set; }
    public Curso Curso { get; set; }
    public string UsuarioId {get; set;}
    public DateTime FechaVisto { get; set; } = DateTime.UtcNow;

}
