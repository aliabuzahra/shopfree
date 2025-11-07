using AutoMapper;
using ShopFree.Application.DTOs.Orders;
using ShopFree.Application.DTOs.PaymentMethods;
using ShopFree.Application.DTOs.Products;
using ShopFree.Application.DTOs.Stores;
using ShopFree.Domain.Entities;
using ShopFree.Domain.ValueObjects;

namespace ShopFree.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Store mappings
        CreateMap<Store, StoreDto>();

        // Product mappings
        CreateMap<Product, ProductDto>();

        // Order mappings
        CreateMap<Order, OrderDto>()
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.Name))
            .ForMember(dest => dest.CustomerEmail, opt => opt.MapFrom(src => src.Customer.Email))
            .ForMember(dest => dest.CustomerPhone, opt => opt.MapFrom(src => src.Customer.Phone))
            .ForMember(dest => dest.ShippingAddress, opt => opt.MapFrom(src => src.ShippingAddress.GetFullAddress()));

        CreateMap<OrderItem, OrderItemDto>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name));

        // PaymentMethod mappings
        CreateMap<PaymentMethod, PaymentMethodDto>();
    }
}

