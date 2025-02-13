using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Identity;

namespace uni.learn.core.Entity;

public class Usuario : IdentityUser
{
    public string Nombre { get; set; }
    public string ApellidoPaterno { get; set; }
    public string ApellidoMaterno { get; set; }
    public bool Admin { get; set; }
    public bool Verificado {get; set;}
    public bool GrupoEstudiantil { get; set; }
    public int Matricula { get; set; }
    public string? Foto { get; set; }
}
