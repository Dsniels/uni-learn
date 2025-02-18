using System;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using AutoMapper.Configuration.Annotations;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using uni.learn.api.EntitiesDto;
using uni.learn.core.Entities;
using uni.learn.tests.Services;

namespace uni.learn.tests;
[Collection("Tests")]

public class TemasTests : IClassFixture<WebApplicationFactory<Program>>
{

    private readonly HttpClient _clientNoAuth;
    private readonly HttpClient _clientUser;
    private readonly HttpClient _clientAdmin;

    public TemasTests(WebApplicationFactory<Program> factory)
    {
        var authService = new AuthService(factory);
        _clientAdmin = authService.CreateClientWithAuth("admin@unilearn.com", "Admin123*").Result;
        _clientUser = authService.CreateClientWithAuth("juan.perez1@ejemplo.com", "Password123!").Result;
        _clientNoAuth = authService.CreateClientNoAuth();
    }

    [Fact]
    public async Task GetAll_ShouldReturnOk()
    {
        var response = await _clientUser.GetAsync("/api/Temas/GetAll");
        response.EnsureSuccessStatusCode();
        var temas = await response.Content.ReadFromJsonAsync<List<Temas>>();

        Assert.NotEmpty(temas);
        Assert.IsType<List<Temas>>(temas);
    }

    [Fact]
    public async Task DeleteByID_ShouldReturnNotFound()
    {
        var response = await _clientAdmin.DeleteAsync($"/api/Temas/DeleteByID/{2}");


        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }


    [Fact]
    public async Task GetByID_ShouldReturnOk()
    {
        var response = await _clientNoAuth.GetAsync($"/api/Temas/GetByID/{1}");

        response.EnsureSuccessStatusCode();
        var tema = await response.Content.ReadFromJsonAsync<Temas>();


        Assert.IsType<Temas>(tema);
    }


    [Fact]
    public async Task Add_ShoulReturnOkAndDelete()
    {
        var newTema = new TemaDto
        {
            Name = "C++"
        };
        var response = await _clientUser.PostAsJsonAsync("/api/Temas/Add", newTema);
        response.EnsureSuccessStatusCode();

        var tema = await response.Content.ReadFromJsonAsync<Temas>();
        Assert.NotNull(tema);
        Assert.Equal(newTema.Name, tema.Nombre);

        var responseDelete = await _clientAdmin.DeleteAsync($"/api/Temas/DeleteByID/{tema.Id}");
        responseDelete.EnsureSuccessStatusCode();


    }
    [Fact]
    public async Task Add_ShoulReturnBadRequestAlreadyExists()
    {
        var newTema = new TemaDto
        {
            Name = "Programación"
        };
        var response = await _clientUser.PostAsJsonAsync("/api/Temas/Add", newTema);
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);

    }



    [Fact]
    public async Task GetByID_ShouldReturnNotFound()
    {
        var response = await _clientNoAuth.GetAsync($"/api/Temas/GetByID/{2}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }


}
