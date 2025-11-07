using MediatR;
using ShopFree.Application.DTOs.Products;

namespace ShopFree.Application.Features.Products.Queries.GetProductById;

public class GetProductByIdQuery : IRequest<ProductDto?>
{
    public int Id { get; set; }
}

