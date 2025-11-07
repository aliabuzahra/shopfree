namespace ShopFree.Application.DTOs.Stores;

public class StoreDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Subdomain { get; set; }
    public string? LogoUrl { get; set; }
    public DateTime CreatedAt { get; set; }
}

