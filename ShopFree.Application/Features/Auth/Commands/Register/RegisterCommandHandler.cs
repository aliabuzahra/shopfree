using MediatR;
using Microsoft.Extensions.Logging;
using ShopFree.Application.DTOs.Auth;
using ShopFree.Domain.Entities;
using ShopFree.Domain.Interfaces;
using ShopFree.Domain.Interfaces.Repositories;
using BCrypt.Net;

namespace ShopFree.Application.Features.Auth.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponseDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtService _jwtService;
    private readonly ILogger<RegisterCommandHandler> _logger;
    
    public RegisterCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IJwtService jwtService,
        ILogger<RegisterCommandHandler> logger)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _jwtService = jwtService;
        _logger = logger;
    }
    
    public async Task<AuthResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        // Check if user already exists
        if (await _userRepository.EmailExistsAsync(request.Email, cancellationToken))
        {
            throw new InvalidOperationException("User with this email already exists");
        }
        
        // Hash password
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        
        // Create user
        var user = new User(request.Email, passwordHash, request.FirstName, request.LastName);
        
        await _userRepository.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        // Generate token
        var token = _jwtService.GenerateToken(user.Id, user.Email);
        
        return new AuthResponseDto
        {
            Token = token,
            User = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            }
        };
    }
}

