using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ShopFree.Application.DTOs.PaymentMethods;
using ShopFree.Domain.Interfaces.Repositories;

namespace ShopFree.Application.Features.PaymentMethods.Queries.GetPaymentMethodsByStoreId;

public class GetPaymentMethodsByStoreIdQueryHandler : IRequestHandler<GetPaymentMethodsByStoreIdQuery, List<PaymentMethodDto>>
{
    private readonly IPaymentMethodRepository _paymentMethodRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetPaymentMethodsByStoreIdQueryHandler> _logger;
    
    public GetPaymentMethodsByStoreIdQueryHandler(
        IPaymentMethodRepository paymentMethodRepository,
        IMapper mapper,
        ILogger<GetPaymentMethodsByStoreIdQueryHandler> logger)
    {
        _paymentMethodRepository = paymentMethodRepository;
        _mapper = mapper;
        _logger = logger;
    }
    
    public async Task<List<PaymentMethodDto>> Handle(GetPaymentMethodsByStoreIdQuery request, CancellationToken cancellationToken)
    {
        var paymentMethods = request.ActiveOnly
            ? await _paymentMethodRepository.GetActiveByStoreIdAsync(request.StoreId, cancellationToken)
            : await _paymentMethodRepository.GetByStoreIdAsync(request.StoreId, cancellationToken);
        
        return _mapper.Map<List<PaymentMethodDto>>(paymentMethods);
    }
}

