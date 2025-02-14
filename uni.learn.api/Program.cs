using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using uni.learn.api;
using uni.learn.api.Middleware;
using uni.learn.BussinesLogic.Context;
using uni.learn.BussinesLogic.Context.Data;
using uni.learn.BussinesLogic.Data;
using uni.learn.core.Entity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();


builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin();
    });
});
builder.Services.AddInfraestructure(builder.Configuration);


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "UniLearn API V1");
    });
}

using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider;
    var context = service.GetRequiredService<MainDbContext>();
    await context.Database.MigrateAsync();
    await MainDbContextData.seedDataAsync(context);

    var IdentityContext = service.GetRequiredService<SecurityDbContext>();
    var userManager = service.GetRequiredService<UserManager<Usuario>>();
    var roleManager = service.GetRequiredService<RoleManager<IdentityRole>>();
    await IdentityContext.Database.MigrateAsync();
    await SecurityDbContextData.SeedUserAsync(userManager, roleManager);
}



app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();
app.UseMiddleware<ExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


app.Run();

