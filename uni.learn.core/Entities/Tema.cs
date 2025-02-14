using System;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using uni.learn.core.Entity;

namespace uni.learn.core.Entities;

public class Temas : Base
{
    public string Nombre {get; set;}
    [JsonIgnore]
    public List<Curso> Cursos {get; set;} = new List<Curso>();

}
