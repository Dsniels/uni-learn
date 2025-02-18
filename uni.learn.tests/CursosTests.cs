using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using uni.learn.api.EntitiesDto;
using System.Net.Http.Headers;
using System.Net;
using uni.learn.core.Entity;
using System.Runtime.CompilerServices;
using uni.learn.BussinesLogic.Migrations;
using uni.learn.core.Entities;

namespace uni.learn.tests;


[Collection("Tests")]
public class CursosTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly HttpClient _clientNoAuth;
    private readonly HttpClient _clientAdmin;
    private int CursoID;

    public CursosTests(WebApplicationFactory<Program> _factory)
    {
        _clientNoAuth = _factory.CreateClient();
        _clientAdmin = _factory.CreateClient();
        _client = _factory.CreateClient();
        SetUpAuthAdmin().GetAwaiter().GetResult();
        SetUpAuthMortalUser().GetAwaiter().GetResult();

    }

    private async Task SetUpAuthAdmin()
    {
        var credentials = new LoginDto
        {
            Email = "admin@unilearn.com",
            Password = "Admin123*"
        };
        var token = await GetTokenAsync(_clientAdmin, credentials);
        _clientAdmin.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
    private async Task SetUpAuthMortalUser()
    {
        var credentials = new LoginDto
        {
            Email = "juan.perez1@ejemplo.com",
            Password = "Password123!"
        };
        var token = await GetTokenAsync(_client, credentials);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    private async Task<string> GetTokenAsync(HttpClient client, LoginDto credentials)
    {

        var response = await client.PostAsJsonAsync("/api/Auth/Login", credentials);
        response.EnsureSuccessStatusCode();

        var body = await response.Content.ReadFromJsonAsync<LoginResponse>();
        Assert.NotNull(body);
        return body.Token;

    }

    [Fact]
    public async Task GetUnApprovedCursos_ReturnUnAuthorized()
    {

        var response = await _clientAdmin.GetAsync("/api/Cursos/GetUnApprovedCursos");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);


    }


    [Fact]
    public async Task GetAllCursos_ReturnsOkResults()
    {

        var response = await _clientAdmin.GetAsync("/api/Cursos/GetAll");
        response.EnsureSuccessStatusCode();

        var cursos = await response.Content.ReadFromJsonAsync<List<CursoDto>>();
        Assert.NotNull(cursos);
        Assert.IsType<List<CursoDto>>(cursos);

    }

    [Fact]
    public async Task GetAllCursosWithSpec_ReturnsOkResults()
    {
        var response = await _clientAdmin.GetAsync("/api/Cursos/GetAll?pageIndex=1&pageSize=10&search=Intro&orderBy=titulo");
        response.EnsureSuccessStatusCode();

        var cursos = await response.Content.ReadFromJsonAsync<List<CursoDto>>();
        Assert.NotNull(cursos);
        Assert.IsType<List<CursoDto>>(cursos);
        Assert.All(cursos, curso =>
        {
            var tituloContains = curso.Titulo != null && curso.Titulo.Contains("Intro", StringComparison.OrdinalIgnoreCase);
            var descripcionContains = curso.Descripcion != null && curso.Descripcion.Contains("Intro", StringComparison.OrdinalIgnoreCase);
            Assert.True(tituloContains || descripcionContains);
        });
    }


    [Fact]
    public async Task GetApprovedCursos_ReturnOk()
    {

        var response = await _clientAdmin.GetAsync("/api/Cursos/GetApprovedCursos");

        response.EnsureSuccessStatusCode();

        var cursos = await response.Content.ReadFromJsonAsync<List<CursoDto>>();
        Assert.NotNull(cursos);
        Assert.IsType<List<CursoDto>>(cursos);
    }

    [Fact]
    public async Task GetCursoByID_ShouldReturnOk()
    {
        var response = await _client.GetAsync($"/api/Cursos/GetByID/{5}");
        response.EnsureSuccessStatusCode();


        var curso = await response.Content.ReadFromJsonAsync<CursoDetailDto>();

        Assert.NotNull(curso);
        Assert.IsType<CursoDetailDto>(curso);
        Assert.IsType<UsuarioDto>(curso.Author);

    }
    [Fact]
    public async Task GetCursoByID_ShouldReturnNotFound()
    {
        var response = await _client.GetAsync($"/api/Cursos/GetByID/{2}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
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

        var response = await _clientAdmin.PostAsJsonAsync("/api/Cursos/Add", curso);
        response.EnsureSuccessStatusCode();

        var cursoCreated = await response.Content.ReadFromJsonAsync<Curso>();
        Assert.NotNull(cursoCreated);
        Assert.IsType<Curso>(cursoCreated);
        Assert.IsType<int>(cursoCreated.Id);
        Assert.Equal(curso.Titulo, cursoCreated.Titulo);
        CursoID = cursoCreated.Id;


        var responseDelete = await _clientAdmin.DeleteAsync($"/api/Cursos/DeleteByID/{cursoCreated.Id}");
        responseDelete.EnsureSuccessStatusCode();

    }

    [Fact]
    public async Task UpdateCurso_ShouldReturnUnathorized()
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
        Assert.IsType<Curso>(cursoCreated);
        Assert.IsType<int>(cursoCreated.Id);
        Assert.Equal(curso.Titulo, cursoCreated.Titulo);


        cursoCreated.Aprobado = true;
        var responseUpdate = await _client.PostAsJsonAsync($"/api/Cursos/UpdateCurso/{cursoCreated.Id}", cursoCreated);
        Assert.Equal(HttpStatusCode.Unauthorized, responseUpdate.StatusCode);
        var UpdateBodyResponse = await responseUpdate.Content.ReadAsStringAsync();
        Assert.Equal("Only an admin can approve this video", UpdateBodyResponse);
        var responseDelete = await _client.DeleteAsync($"/api/Cursos/DeleteByID/{cursoCreated.Id}");
        responseDelete.EnsureSuccessStatusCode();

    }
    [Fact]
    public async Task UpdateCurso_ShouldReturnCursoNotFound()
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



        var responseUpdate = await _client.PostAsJsonAsync($"/api/Cursos/UpdateCurso/{2}", curso);
        Assert.Equal(HttpStatusCode.BadRequest, responseUpdate.StatusCode);


    }



    [Fact]
    public async Task UpdateCurso_ReturnOk()
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

        var response = await _clientAdmin.PostAsJsonAsync("/api/Cursos/Add", curso);
        response.EnsureSuccessStatusCode();
        var cursoCreated = await response.Content.ReadFromJsonAsync<Curso>();


        Assert.NotNull(cursoCreated);
        Assert.IsType<Curso>(cursoCreated);
        Assert.IsType<int>(cursoCreated.Id);
        Assert.Equal(curso.Titulo, cursoCreated.Titulo);


        var newTitulo = "Introduccion a Asp.Net Core 9 Web Api";
        var newDescription = "Curso completo de ASP.NET Core Web API";
        cursoCreated.Titulo = newTitulo;
        cursoCreated.Descripcion = newDescription;


        var responseUpdate = await _clientAdmin.PostAsJsonAsync($"/api/Cursos/UpdateCurso/{cursoCreated.Id}", cursoCreated);
        responseUpdate.EnsureSuccessStatusCode();

        var UpdateBodyResponse = await responseUpdate.Content.ReadFromJsonAsync<Curso>();
        Assert.IsType<Curso>(UpdateBodyResponse);
        Assert.Equal(newTitulo, UpdateBodyResponse.Titulo);
        Assert.Equal(newDescription, UpdateBodyResponse.Descripcion);
        var responseDelete = await _clientAdmin.DeleteAsync($"/api/Cursos/DeleteByID/{cursoCreated.Id}");
        responseDelete.EnsureSuccessStatusCode();

    }

    [Fact]
    public async Task DeleteCurso_ReturnsUnauthorized_ForDifferentUser()
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

        var response = await _clientAdmin.PostAsJsonAsync("/api/Cursos/Add", curso);

        response.EnsureSuccessStatusCode();

        var cursoCreated = await response.Content.ReadFromJsonAsync<Curso>();


        Assert.NotNull(cursoCreated);
        Assert.Equal(curso.Titulo, cursoCreated.Titulo);
        CursoID = cursoCreated.Id;
        var responseDelete = await _clientAdmin.DeleteAsync($"/api/Cursos/DeleteByID/{cursoCreated.Id}");

        responseDelete.EnsureSuccessStatusCode();

    }

    [Fact]
    public async Task DeleteCurso_ReturnNotFound()
    {


        var responseDelete = await _clientAdmin.DeleteAsync($"/api/Cursos/DeleteByID/{2}");

        Assert.Equal(HttpStatusCode.NotFound, responseDelete.StatusCode);

    }
    [Fact]
    public async Task DeleteCurso_ReturnUnAuthorized()
    {
        var responseDelete = await _clientNoAuth.DeleteAsync($"/api/Cursos/DeleteByID/{1}");
        Assert.Equal(HttpStatusCode.Unauthorized, responseDelete.StatusCode);

    }

    [Fact]
    public async Task GetCursosVistos_ShouldReturnOk()
    {
        var response = await _client.GetAsync("/api/Cursos/GetCursosVistos");
        //var cursos = await response.Content.ReadAsStringAsync();
        var cursos = await response.Content.ReadFromJsonAsync<IReadOnlyCollection<CursoVisto>>();
        System.Console.WriteLine(cursos);

        Assert.IsType<List<CursoVisto>>(cursos);

    }


  
}
