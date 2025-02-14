using System;
using uni.learn.core.Entity;

namespace uni.learn.api.EntitiesDto;

public class CursoDetailDto : Curso
{
    public Usuario Author {get; set;}
    

}
