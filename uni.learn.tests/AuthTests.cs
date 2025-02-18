using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using uni.learn.api;
using uni.learn.api.EntitiesDto;
using uni.learn.core.Entity;
using uni.learn.core.Specifications;
namespace uni.learn.tests;

[Collection("Tests")]
public class AuthTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    public AuthTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Login_ReturnTokenRoleAndUser()
    {
        HttpClient cliente = _factory.CreateClient();
        var loginRequest = new LoginDto
        {
            Email = "admin@unilearn.com",
            Password = "Admin123*"
        };
        var response = await cliente.PostAsJsonAsync("/api/Auth/Login", loginRequest);

        response.EnsureSuccessStatusCode( );

        var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
        Assert.NotNull(loginResponse);
        Assert.NotNull(loginResponse.Usuario);
        Assert.IsType<bool>(loginResponse.Admin);
        Assert.NotNull(loginResponse.Token);


    }


    
    [Fact]
    public async Task Login_ReturnPasswordOrEmailIncorrect()
    {
        HttpClient cliente = _factory.CreateClient();
        var loginRequest = new LoginDto
        {
            Email = "admin@unilearn.com",
            Password = "dmin123*"
        };
        var response = await cliente.PostAsJsonAsync("/api/Auth/Login", loginRequest);


        var loginResponse = await response.Content.ReadAsStringAsync();
        Assert.NotNull(loginResponse);
        Assert.Equal("Password or Email are incorrect", loginResponse);

    }


 

}
