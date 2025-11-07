using MediatR;
using ShopFree.Application.DTOs.Stores;

namespace ShopFree.Application.Features.Stores.Queries.GetStoreById;

public class GetStoreByIdQuery : IRequest<StoreDto?>
{
    public int Id { get; set; }
}

