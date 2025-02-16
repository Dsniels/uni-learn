using System;
using uni.learn.core.Entity;

namespace uni.learn.api.EntitiesDto;

public class CursoDetailDto : Curso
{
    public UsuarioDto Author {get; set;}
    

}
