using System;
using uni.learn.core.Entities;

namespace uni.learn.core.Entity;

public class Curso : Base
{

    public string Titulo { get; set; }
    public string AuthorId { get; set; }
    public string Descripcion { get; set; }
    public List<Temas>? Temas { get; set; }
    public string Video { get; set; }
    public int Vistas { get; set; }
    public bool Aprobado { get; set; }


}
