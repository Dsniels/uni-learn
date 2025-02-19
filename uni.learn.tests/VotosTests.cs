using System;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using uni.learn.api.EntitiesDto;
using uni.learn.tests.Services;

namespace uni.learn.tests;

public class VotosTests : IClassFixture<WebApplicationFactory<Program>>
{

    private readonly HttpClient _client;
    private readonly HttpClient _clientNoAuth;
    private readonly HttpClient _clientAdmin;

    public VotosTests(WebApplicationFactory<Program> factory)
    {
        var authService = new AuthService(factory);
        _clientAdmin = authService.CreateClientWithAuth("admin@unilearn.com", "Admin123*").Result;
        _client = authService.CreateClientWithAuth("juan.perez1@ejemplo.com", "Password123!").Result;
        _clientNoAuth = authService.CreateClientNoAuth();
    }

    [Fact]
    public async Task GetVotos_ShouldReturnOk()
    {
        var response = await _client.GetAsync($"/api/Votos/GetVotos?cursoId=5");

        response.EnsureSuccessStatusCode();

    }



    [Fact]
    public async Task LikeCurso_ShouldReturnOkAndCreate(){
        var response = await _client.PostAsync("/api/Votos/likeCurso?cursoId=5", null);

        response.EnsureSuccessStatusCode();

        

    }


    [Fact]
    public async Task LikeCurso_ShouldReturnOk(){
        var response = await _client.PostAsync("/api/Votos/likeCurso?cursoId=5", null);

        response.EnsureSuccessStatusCode();

        

    }
    [Fact]
    public async Task UserLikeCurso_ShouldReturnTrue(){

        var response = await _client.GetAsync("/api/Votos/UserLikedCurso?cursoId=5");
        response.EnsureSuccessStatusCode();

        var like = await response.Content.ReadFromJsonAsync<bool>();

        Assert.True(like);

    }


    [Fact]
    public async Task DisLikeCurso_ShouldReturnOk(){
        var response = await _client.PostAsync("/api/Votos/DisLikeCurso?cursoId=5", null);

        response.EnsureSuccessStatusCode();

        

    }


}
