using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using uni.learn.core.Entity;

namespace uni.learn.api.Extensions;

public static class UserManagerExtension
{
    public static async Task<Usuario> BuscarUsuarioAsync(this UserManager<Usuario> input, ClaimsPrincipal user)
    {
        var email = user?.Claims?.FirstOrDefault(e => e.Type == ClaimTypes.Email)?.Value;
        return await input.Users.SingleOrDefaultAsync(x => x.Email == email);
    }

}
