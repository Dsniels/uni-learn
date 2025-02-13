using System;
using System.ComponentModel.DataAnnotations;

namespace uni.learn.api.EntitiesDto;

public class RegistroDTO
{
    public string Nombre { get; set; }
    public string ApellidoPaterno { get; set; }
    public string ApellidoMaterno { get; set; }
    public int Matricula { get; set; }
    public string UserName { get; set; }
    public string Imagen {get; set;}
    public string Email { get; set; }
    public string Password { get; set; }
}
