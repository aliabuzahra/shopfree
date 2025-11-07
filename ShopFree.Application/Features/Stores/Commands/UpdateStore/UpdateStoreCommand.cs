using MediatR;
using ShopFree.Application.DTOs.Stores;

namespace ShopFree.Application.Features.Stores.Commands.UpdateStore;

public class UpdateStoreCommand : IRequest<StoreDto>
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? LogoUrl { get; set; }
}

