using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using uni.learn.BussinesLogic.Context;
using uni.learn.BussinesLogic.Data;
using uni.learn.core.Entity;

namespace uni.learn.api;

public static class ServicesCollectionsExtensions
{
    public static void AddInfraestructure(this IServiceCollection services, IConfiguration configuration)
    {

        var connectionString = configuration.GetConnectionString("DefaultConnection");
        var IdentityConnectionString = configuration.GetConnectionString("SecurityConnection");
        var IssuerString = configuration["Token:Key"];


        services.AddDbContext<MainDbContext>(opt => opt.UseSqlServer(connectionString));
        services.AddDbContext<SecurityDbContext>(opt => opt.UseSqlServer(IdentityConnectionString));


        var builder = services.AddIdentityCore<Usuario>();
        builder = new IdentityBuilder(builder.UserType, builder.Services);
        builder.AddRoles<IdentityRole>();
        builder.AddEntityFrameworkStores<SecurityDbContext>();
        builder.AddSignInManager<SignInManager<Usuario>>();


        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
            options.UseSecurityTokenValidators = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Token:Key"])),
                ValidIssuer = configuration["Token:Issuer"],
                ValidateIssuer = true,
                ValidateAudience = false,
            };
        });

        services.AddAuthorization();
        services.AddCors(opt =>
        {
            opt.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyHeader();
                builder.AllowAnyOrigin();
                builder.AllowAnyMethod();
            });
        });









    }

}
