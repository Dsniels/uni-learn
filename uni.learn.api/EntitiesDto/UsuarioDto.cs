using System;

namespace uni.learn.api.EntitiesDto;

public class UsuarioDto
{
    public string Id { get; set; }
    public string Nombre { get; set; }
    public string ApellidoPaterno { get; set; }
    public string ApellidoMaterno { get; set; }
    public string Email { get; set; }
    public string Imagen { get; set; }
    public int Matricula {get; set;}
    // public bool Admin { get; set; }
}
