using MediatR;
using ShopFree.Application.DTOs.Stores;

namespace ShopFree.Application.Features.Stores.Queries.GetStoresByUserId;

public class GetStoresByUserIdQuery : IRequest<List<StoreDto>>
{
    public int UserId { get; set; }
}

