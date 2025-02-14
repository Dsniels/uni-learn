using System;
using uni.learn.core.Entity;

namespace uni.learn.core.Entities;

public class Voto : Base
{
    public int CursoId { get; set; }
    public Curso Curso { get; set; }
    public string UsuarioId { get; set; }
    public Usuario Usuario { get; set; }
    public bool Like { get; set; }
    public DateTime FechaVoto { get; set; } = DateTime.UtcNow;

}
