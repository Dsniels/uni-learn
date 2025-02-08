using uni.learn.api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddInfraestructure(builder.Configuration);


var app = builder.Build();
app.MapOpenApi();
app.UseCors();
app.UseRouting();
// app.UseMiddleware();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(e=>{
    e.MapControllers();
});

app.UseHttpsRedirection();
app.Run();

