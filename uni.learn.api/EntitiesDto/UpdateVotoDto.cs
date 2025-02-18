using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;

namespace uni.learn.api.EntitiesDto;

public class UpdateVotoDto
{
    [Required]
    public int CursoId{get;set;}
    public bool like {get; set;}

}
