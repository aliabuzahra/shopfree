using System.Net;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using FluentAssertions;
using ShopFree.IntegrationTests;
using Xunit;

namespace ShopFree.IntegrationTests.Controllers;

public class StoresControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public StoresControllerTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    private async Task<string> GetAuthTokenAsync()
    {
        // Register and login to get token
        var email = $"storetest{Guid.NewGuid()}@example.com";
        var password = "Password123!";
        
        var registerDto = new
        {
            email = email,
            password = password,
            firstName = "Test",
            lastName = "User"
        };
        await _client.PostAsJsonAsync("/api/auth/register", registerDto);

        var loginDto = new
        {
            email = email,
            password = password
        };
        var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", loginDto);
        var loginResult = await loginResponse.Content.ReadFromJsonAsync<dynamic>();
        
        return loginResult?.token?.ToString() ?? string.Empty;
    }

    [Fact]
    public async Task CreateStore_WithValidData_ShouldReturnCreated()
    {
        // Arrange
        var token = await GetAuthTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var storeDto = new
        {
            name = "Test Store",
            description = "Test Description",
            subdomain = $"store{Guid.NewGuid():N}",
            logoUrl = "https://example.com/logo.png"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/stores", storeDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var result = await response.Content.ReadFromJsonAsync<dynamic>();
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateStore_WithoutAuth_ShouldReturnUnauthorized()
    {
        // Arrange
        var storeDto = new
        {
            name = "Test Store",
            description = "Test Description"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/stores", storeDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetMyStores_WithAuth_ShouldReturnStores()
    {
        // Arrange
        var token = await GetAuthTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Create a store first
        var storeDto = new
        {
            name = "Test Store",
            description = "Test Description",
            subdomain = $"store{Guid.NewGuid():N}"
        };
        await _client.PostAsJsonAsync("/api/stores", storeDto);

        // Act
        var response = await _client.GetAsync("/api/stores/my-stores");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}

