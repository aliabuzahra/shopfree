using FluentAssertions;
using Moq;
using ShopFree.Application.Features.Auth.Commands.Register;
using ShopFree.Domain.Entities;
using ShopFree.Domain.Interfaces;
using ShopFree.Domain.Interfaces.Repositories;
using Xunit;

namespace ShopFree.UnitTests.Features.Auth;

public class RegisterCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IJwtService> _jwtServiceMock;
    private readonly RegisterCommandHandler _handler;

    public RegisterCommandHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _jwtServiceMock = new Mock<IJwtService>();
        
        _handler = new RegisterCommandHandler(
            _userRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _jwtServiceMock.Object,
            Mock.Of<Microsoft.Extensions.Logging.ILogger<RegisterCommandHandler>>());
    }

    [Fact]
    public async Task Handle_WhenUserDoesNotExist_ShouldCreateUserAndReturnToken()
    {
        // Arrange
        var command = new RegisterCommand
        {
            Email = "test@example.com",
            Password = "Password123!",
            FirstName = "Test",
            LastName = "User"
        };

        _userRepositoryMock
            .Setup(x => x.EmailExistsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _jwtServiceMock
            .Setup(x => x.GenerateToken(It.IsAny<int>(), It.IsAny<string>()))
            .Returns("test-token");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Token.Should().Be("test-token");
        result.User.Email.Should().Be(command.Email);
        
        _userRepositoryMock.Verify(x => x.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenUserExists_ShouldThrowException()
    {
        // Arrange
        var command = new RegisterCommand
        {
            Email = "existing@example.com",
            Password = "Password123!"
        };

        _userRepositoryMock
            .Setup(x => x.EmailExistsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(command, CancellationToken.None));
    }
}

