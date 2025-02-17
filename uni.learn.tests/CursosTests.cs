using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using uni.learn.api.EntitiesDto;
using System.Net.Http.Headers;
using System.Net;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using uni.learn.core.Entity;

namespace uni.learn.tests;

public class CursosTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    private int CursoID;

    public CursosTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        SetUpAuth().GetAwaiter().GetResult();

    }

    private async Task SetUpAuth()
    {
        var credentials = new LoginDto
        {
            Email = "admin@unilearn.com",
            Password = "Admin123*"
        };
        var token = await GetTokenAsync(credentials);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    private async Task<string> GetTokenAsync(LoginDto credentials)
    {

        var response = await _client.PostAsJsonAsync("/api/Auth/Login", credentials);
        response.EnsureSuccessStatusCode();

        var body = await response.Content.ReadFromJsonAsync<LoginResponse>();
        Assert.NotNull(body);
        return body.Token;

    }

    [Fact]
    public async Task GetUnApprovedCursos_ReturnUnAuthorized()
    {


        var response = await _client.GetAsync("/api/Cursos/GetUnApprovedCursos");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);


    }


    [Fact]
    public async Task GetAllCursos_ReturnsOkResults()
    {

        var response = await _client.GetAsync("/api/Cursos/GetAll");

        response.EnsureSuccessStatusCode();

        var cursos = await response.Content.ReadFromJsonAsync<List<CursoDto>>();
        Assert.NotNull(cursos);
        Assert.IsType<List<CursoDto>>(cursos);
    }



    [Fact]
    public async Task GetApprovedCursos_ReturnOk()
    {

        var response = await _client.GetAsync("/api/Cursos/GetApprovedCursos");

        response.EnsureSuccessStatusCode();

        var cursos = await response.Content.ReadFromJsonAsync<List<CursoDto>>();
        Assert.NotNull(cursos);
        Assert.IsType<List<CursoDto>>(cursos);
    }



    [Fact]
    public async Task AddCurso_ReturnOk()
    {

        var curso = new CursoDto
        {
            Titulo = "Introducción a ASP.NET Core",
            Author = "eba0dea4-1811-4f70-bbdd-5e30c0ebaa9f",
            Video = "https://youtube.com/watch?v=example",
            Descripcion = "Curso completo de ASP.NET Core para principiantes",
            Temas = new List<TemaDto>
        {
            new TemaDto { Id = 1 },
            new TemaDto { Id = 8 }
        }
        };

        var response = await _client.PostAsJsonAsync("/api/Cursos/Add", curso);

        response.EnsureSuccessStatusCode();

        var cursoCreated = await response.Content.ReadFromJsonAsync<Curso>();


        Assert.NotNull(cursoCreated);
        Assert.Equal(curso.Titulo, cursoCreated.Titulo);
        CursoID = cursoCreated.Id;
        var responseDelete = await _client.DeleteAsync($"/api/Cursos/DeleteByID/{cursoCreated.Id}");

        responseDelete.EnsureSuccessStatusCode();

    }


    [Fact]
    public async Task DeleteCurso_ReturnOk()
    {

        var curso = new CursoDto
        {
            Titulo = "Introducción a Angular",
            Author = "eba0dea4-1811-4f70-bbdd-5e30c0ebaa9f",
            Video = "https://youtube.com/watch?v=example",
            Descripcion = "Curso completo de Angular para principiantes",
            Temas = new List<TemaDto>
        {
            new TemaDto { Id = 1 },
            new TemaDto { Id = 8 }
        }
        };

        var response = await _client.PostAsJsonAsync("/api/Cursos/Add", curso);

        response.EnsureSuccessStatusCode();

        var cursoCreated = await response.Content.ReadFromJsonAsync<Curso>();


        Assert.NotNull(cursoCreated);
        Assert.Equal(curso.Titulo, cursoCreated.Titulo);
        CursoID = cursoCreated.Id;
        var responseDelete = await _client.DeleteAsync($"/api/Cursos/DeleteByID/{cursoCreated.Id}");

        responseDelete.EnsureSuccessStatusCode();

    }

}
