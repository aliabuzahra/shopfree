namespace ShopFree.Domain.Interfaces;

public interface IJwtService
{
    string GenerateToken(int userId, string email);
}

