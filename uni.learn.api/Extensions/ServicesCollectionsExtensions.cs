using System;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using uni.learn.api.EntitiesDto;
using uni.learn.BussinesLogic.Context;
using uni.learn.BussinesLogic.Data;
using uni.learn.BussinesLogic.Logic;
using uni.learn.core.Entity;
using uni.learn.core.Interfaces;

namespace uni.learn.api;

public static class ServicesCollectionsExtensions
{
    public static void AddInfraestructure(this IServiceCollection services, IConfiguration configuration)
    {

        var connectionString = configuration.GetConnectionString("DefaultConnectionLocal");
        var IdentityConnectionString = configuration.GetConnectionString("SecurityConnectionlocal");
        var IssuerString = configuration["Token:Key"];


        services.AddDbContext<MainDbContext>(opt => opt.UseSqlServer(connectionString));
        services.AddDbContext<SecurityDbContext>(opt => opt.UseSqlServer(IdentityConnectionString));
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped(typeof(IGenericSecurityRepository<>), typeof(GenericSecurityRepository<>));
        services.AddScoped<ITokenService, TokenService>();
        services.AddAutoMapper(typeof(MappingProfiles));
        services.TryAddSingleton<ISystemClock, SystemClock>();
        services.AddTransient<ICursoRepository, CursoRepository>();
        var builder = services.AddIdentityCore<Usuario>();
        builder = new IdentityBuilder(builder.UserType, builder.Services);
        builder.AddRoles<IdentityRole>();
        builder.AddEntityFrameworkStores<SecurityDbContext>();
        builder.AddSignInManager<SignInManager<Usuario>>();

        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
             options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });
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








    }

}
