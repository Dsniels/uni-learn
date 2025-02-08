using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.IdentityModel.Tokens;

namespace uni.learn.api;

public static class ServicesCollectionsExtensions
{
    public static void AddInfraestructure(this IServiceCollection services, IConfiguration configuration)
    {

        var connectionString = configuration.GetConnectionString("DefaultConnection");
        var IdentityConnectionString = configuration.GetConnectionString("IdentityConnection");
        var IssuerString = configuration["Token:Key"];

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
