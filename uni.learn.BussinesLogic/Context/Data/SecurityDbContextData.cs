using System;
using Microsoft.AspNetCore.Identity;
using uni.learn.core.Entity;

namespace uni.learn.BussinesLogic.Context.Data;

public class SecurityDbContextData
{
    public static async Task SeedUserAsync(UserManager<Usuario> userManager, RoleManager<IdentityRole> roleManager)
    {
        if (!roleManager.Roles.Any())
        {
            var role = new IdentityRole
            {
                Name = "ADMIN"
            };

            var roleAlumno = new IdentityRole
            {
                Name = "ALUMNO"
            };
            await roleManager.CreateAsync(role);
            await roleManager.CreateAsync(roleAlumno);

        }

        
        if (!userManager.Users.Any())
        {
            var usuario = new Usuario
            {
                Nombre = "Admin",
                ApellidoPaterno = "Sistema",
                ApellidoMaterno = "Sys",
                UserName = "admin@unilearn.com",
                Email = "admin@unilearn.com",
                EmailConfirmed = true,
                PhoneNumber = "1234567890",
                Matricula = 22222,
                Admin = true
            };

            var resultado = await userManager.CreateAsync(usuario, "Admin123*");

            if (resultado.Succeeded)
            {
                await userManager.AddToRoleAsync(usuario, "ADMIN");
            }
        }



    }

}
