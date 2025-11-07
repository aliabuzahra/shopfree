using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using ShopFree.IntegrationTests;
using Xunit;

namespace ShopFree.IntegrationTests.Controllers;

public class AuthControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AuthControllerTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Register_WithValidData_ShouldReturnSuccess()
    {
        // Arrange
        var registerDto = new
        {
            email = $"test{Guid.NewGuid()}@example.com",
            password = "Password123!",
            firstName = "Test",
            lastName = "User"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/register", registerDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<dynamic>();
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task Register_WithDuplicateEmail_ShouldReturnBadRequest()
    {
        // Arrange
        var email = $"duplicate{Guid.NewGuid()}@example.com";
        var registerDto = new
        {
            email = email,
            password = "Password123!",
            firstName = "Test",
            lastName = "User"
        };

        // Register first time
        await _client.PostAsJsonAsync("/api/auth/register", registerDto);

        // Act - Try to register again with same email
        var response = await _client.PostAsJsonAsync("/api/auth/register", registerDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Login_WithValidCredentials_ShouldReturnToken()
    {
        // Arrange
        var email = $"login{Guid.NewGuid()}@example.com";
        var password = "Password123!";
        
        // Register first
        var registerDto = new
        {
            email = email,
            password = password,
            firstName = "Test",
            lastName = "User"
        };
        await _client.PostAsJsonAsync("/api/auth/register", registerDto);

        // Act
        var loginDto = new
        {
            email = email,
            password = password
        };
        var response = await _client.PostAsJsonAsync("/api/auth/login", loginDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<dynamic>();
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task Login_WithInvalidCredentials_ShouldReturnUnauthorized()
    {
        // Arrange
        var loginDto = new
        {
            email = "nonexistent@example.com",
            password = "WrongPassword"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/login", loginDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}

