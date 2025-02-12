using System;
using uni.learn.core.Entity;

namespace uni.learn.core.Entities;

public class Temas : Base
{

    public string Nombre {get; set;}
    public List<Curso> Cursos {get; set;}

}
