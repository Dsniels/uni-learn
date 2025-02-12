using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using uni.learn.api;
using uni.learn.BussinesLogic.Context;
using uni.learn.BussinesLogic.Context.Data;
using uni.learn.BussinesLogic.Data;
using uni.learn.core.Entity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddInfraestructure(builder.Configuration);


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider;
    var context = service.GetRequiredService<MainDbContext>();
    await context.Database.MigrateAsync();

    var IdentityContext = service.GetRequiredService<SecurityDbContext>();
    var userManager = service.GetRequiredService<UserManager<Usuario>>();
    var roleManager = service.GetRequiredService<RoleManager<IdentityRole>>();
    await IdentityContext.Database.MigrateAsync();
    await SecurityDbContextData.SeedUserAsync(userManager, roleManager);
}



app.MapOpenApi();
app.UseCors();
app.UseRouting();
// app.UseMiddleware();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(e =>
{
    e.MapControllers();
});

app.UseHttpsRedirection();
app.Run();

