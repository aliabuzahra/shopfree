using MediatR;
using ShopFree.Application.DTOs.Stores;

namespace ShopFree.Application.Features.Stores.Commands.CreateStore;

public class CreateStoreCommand : IRequest<StoreDto>
{
    public int UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Subdomain { get; set; }
    public string? LogoUrl { get; set; }
}

