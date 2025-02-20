using System;
using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using uni.learn.api.EntitiesDto;
using uni.learn.tests.Services;

namespace uni.learn.tests;

[Collection("Tests")]

public class UserTests : IClassFixture<WebApplicationFactory<Program>>
{

    private readonly HttpClient _client;
    private readonly HttpClient _clientNoAuth;
    private readonly HttpClient _clientAdmin;


    public UserTests(WebApplicationFactory<Program> factory){
        var authService = new AuthService(factory); 
        _clientAdmin = authService.CreateClientWithAuth("admin@unilearn.com", "Admin123*").Result;
        _client = authService.CreateClientWithAuth("juan.perez1@ejemplo.com", "Password123!").Result;
        _clientNoAuth = authService.CreateClientNoAuth();

    }

    [Fact]
    public async Task GetAllUsers_ShouldReturnOkAndAList(){
        var response = await _client.GetAsync("/api/User/GetAllUsers");

        response.EnsureSuccessStatusCode();

        var list = await response.Content.ReadFromJsonAsync<List<UsuarioDto>>();

        Assert.IsType<List<UsuarioDto>>(list);
    }

    [Fact]
    public async Task GetAllUsers_ShouldReturnForbidden(){
        var response = await _clientNoAuth.GetAsync("/api/User/GetAllUsers");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task AddRoleToUser_WithValidRole_ShouldReturnOk()
    {
        var userId = "eba0dea4-1811-4f70-bbdd-5e30c0ebaa9f";
        var roleDto = new RoleDto { Name = "ADMIN", Status = true };

        var response = await _clientAdmin.PutAsJsonAsync($"/api/User/AddRoleToUser/{userId}", roleDto);
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task AddRoleToUser_WithInvalidRole_ShouldReturnNotFound()
    {
        var userId = "eba0dea4-1811-4f70-bbdd-5e30c0ebaa9f";
        var roleDto = new RoleDto { Name = "INVALID_ROLE", Status = true };

        var response = await _clientAdmin.PutAsJsonAsync($"/api/User/AddRoleToUser/{userId}", roleDto);

        
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task AddRoleToUser_WithInvalidUser_ShouldReturnNotFound()
    {
       
        var userId = "invalid_user_id";
        var roleDto = new RoleDto { Name = "ADMIN", Status = true };

      
        var response = await _clientAdmin.PutAsJsonAsync($"/api/User/AddRoleToUser/{userId}", roleDto);

        
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task AddRoleToUser_WithNonAdminUser_ShouldReturnForbidden()
    {
      
        var userId = "eba0dea4-1811-4f70-bbdd-5e30c0ebaa9f";
        var roleDto = new RoleDto { Name = "ADMIN", Status = true };

       
        var response = await _client.PutAsJsonAsync($"/api/User/AddRoleToUser/{userId}", roleDto);
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task AddRoleToUser_WithNoAuth_ShouldReturnForbidden()
    {
       
        var userId = "eba0dea4-1811-4f70-bbdd-5e30c0ebaa9f";
        var roleDto = new RoleDto { Name = "ADMIN", Status = true };

     
        var response = await _clientNoAuth.PutAsJsonAsync($"/api/User/AddRoleToUser/{userId}", roleDto);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }



    [Fact]
    public async Task GetByID_ShouldReturnUser(){
        var userID = "eba0dea4-1811-4f70-bbdd-5e30c0ebaa9f";
        var response = await _clientNoAuth.GetAsync($"/api/User/GetUserByID/{userID}");

        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadFromJsonAsync<UsuarioDto>();

        Assert.IsType<UsuarioDto>(body);

    }

    // [Fact]
    // public async Task UpdateMyAccount_WithValidData_ShouldReturnOk()
    // {
    //     var updateData = new RegistroDTO
    //     {
    //         Nombre = "Juan Updated",
    //         ApellidoPaterno = "Perez Updated",
    //         ApellidoMaterno = "Garcia",
    //         Matricula = 12345,
    //         Imagen = "photo.jpg"
    //     };

    //     var response = await _client.PutAsJsonAsync("/api/User/account/updateMyAccount", updateData);
        
    //     Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    //     var result = await response.Content.ReadFromJsonAsync<dynamic>();
    //     Assert.NotNull(result);
    // }

    // [Fact]
    // public async Task UpdateMyAccount_WithPasswordChange_ShouldReturnOk()
    // {
      
    //     var updateData = new RegistroDTO
    //     {
    //         Nombre = "Juan",
    //         ApellidoPaterno = "Perez",
    //         ApellidoMaterno = "Garcia",
    //         Password = "NewPassword123!"
    //     };

       
    //     var response = await _client.PutAsJsonAsync("/api/User/account/updateMyAccount", updateData);
        
     
    //     Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    // }

    [Fact]
    public async Task UpdateMyAccount_WithNoAuth_ShouldReturnForbidden()
    {
    
        var updateData = new RegistroDTO
        {
            Nombre = "Juan",
            ApellidoPaterno = "Perez"
        };

      
        var response = await _clientNoAuth.PutAsJsonAsync("/api/User/account/updateMyAccount", updateData);
        
      
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }


    [Fact]
    public async Task GetMyAccount(){
        var response = await _clientAdmin.GetAsync("/api/User/me");
        
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadFromJsonAsync<UsuarioDto>();

        Assert.IsType<UsuarioDto>(body);


    }

    // [Fact]
    // public async Task UpdateMyAccount_ShouldUpdateAndReturnCorrectData()
    // {
      
    //     var updateData = new RegistroDTO
    //     {
    //         Nombre = "Juan Modified",
    //         ApellidoPaterno = "Perez Modified",
    //         ApellidoMaterno = "Garcia Modified",
    //         Matricula =54321
    //     };

     
    //     var response = await _client.PutAsJsonAsync("/api/User/account/updateMyAccount", updateData);
        
      
    //     response.EnsureSuccessStatusCode();
    //     var result = await response.Content.ReadFromJsonAsync<dynamic>();
    //     Assert.NotNull(result.userDto);
    //     Assert.NotNull(result.Admin);
    // }

    // [Fact]
    // public async Task UpdateUserInfo_WithAdminUser_ShouldReturnOk()
    // {
    
    //     var userId = "eba0dea4-1811-4f70-bbdd-5e30c0ebaa9f";
    //     var updateData = new RegistroDTO
    //     {
    //         Nombre = "Test Updated",
    //         ApellidoPaterno = "User Updated",
    //         ApellidoMaterno = "Modified"
    //     };

   
    //     var response = await _clientAdmin.PutAsJsonAsync($"/api/User/account/updateUserInfo/{userId}", updateData);
        
   
    //     response.EnsureSuccessStatusCode();
    //     var result = await response.Content.ReadFromJsonAsync<dynamic>();
    //     Assert.NotNull(result.userDto);
    //     Assert.NotNull(result.Admin);
    // }

    // [Fact]
    // public async Task UpdateUserInfo_WithPasswordChange_ShouldReturnOk()
    // {
    //     var userId = "eba0dea4-1811-4f70-bbdd-5e30c0ebaa9f";
    //     var updateData = new RegistroDTO
    //     {
    //         Nombre = "Test",
    //         ApellidoPaterno = "User",
    //         ApellidoMaterno = "Test",
    //         Password = "NewPassword123!"
    //     };

    //     var response = await _clientAdmin.PutAsJsonAsync($"/api/User/account/updateUserInfo/{userId}", updateData);
        
    //     Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    // }

    // [Fact]
    // public async Task UpdateUserInfo_WithInvalidUserId_ShouldReturnNotFound()
    // {
    //     var userId = "invalid-user-id";
    //     var updateData = new RegistroDTO
    //     {
    //         Nombre = "Test",
    //         ApellidoPaterno = "User"
    //     };

    //     var response = await _clientAdmin.PutAsJsonAsync($"/api/User/account/updateUserInfo/{userId}", updateData);
        
    //     Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    // }

    [Fact]
    public async Task UpdateUserInfo_WithNonAdminUser_ShouldReturnForbidden()
    {
        var userId = "eba0dea4-1811-4f70-bbdd-5e30c0ebaa9f";
        var updateData = new RegistroDTO
        {
            Nombre = "Test",
            ApellidoPaterno = "User"
        };

        var response = await _client.PutAsJsonAsync($"/api/User/account/updateUserInfo/{userId}", updateData);
        
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task UpdateUserInfo_WithNoAuth_ShouldReturnForbidden()
    {
        var userId = "eba0dea4-1811-4f70-bbdd-5e30c0ebaa9f";
        var updateData = new RegistroDTO
        {
            Nombre = "Test",
            ApellidoPaterno = "User"
        };

        var response = await _clientNoAuth.PutAsJsonAsync($"/api/User/account/updateUserInfo/{userId}", updateData);
        
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

}
