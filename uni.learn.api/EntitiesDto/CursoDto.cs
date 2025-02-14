using System;

namespace uni.learn.api.EntitiesDto;

public class CursoDto
{
    public string Titulo { get; set; }
    public string Author { get; set; }
    public List<TemaDto> Temas { get; set; }
    public string Video { get; set; }


}
