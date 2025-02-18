using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using uni.learn.api.EntitiesDto;

namespace uni.learn.tests.Services;

public class AuthService
{
    private readonly WebApplicationFactory<Program> _factory;

    public AuthService(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    public async Task<HttpClient> CreateClientWithAuth(string email, string password)
    {
        var client = _factory.CreateClient();
        var token = await GetTokenAsync(client, new LoginDto
        {
            Email = email,
            Password = password
        });
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return client;
    }

    public HttpClient CreateClientNoAuth()
    {
        return _factory.CreateClient();
    }

    private async Task<string> GetTokenAsync(HttpClient client, LoginDto credentials)
    {
        var response = await client.PostAsJsonAsync("/api/Auth/Login", credentials);
        response.EnsureSuccessStatusCode();

        var body = await response.Content.ReadFromJsonAsync<LoginResponse>();
        Assert.NotNull(body);
        return body.Token;
    }
}
