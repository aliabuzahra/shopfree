using MediatR;
using ShopFree.Application.DTOs.Products;

namespace ShopFree.Application.Features.Products.Queries.GetProductsByStoreId;

public class GetProductsByStoreIdQuery : IRequest<List<ProductDto>>
{
    public int StoreId { get; set; }
    public bool ActiveOnly { get; set; } = false;
}

