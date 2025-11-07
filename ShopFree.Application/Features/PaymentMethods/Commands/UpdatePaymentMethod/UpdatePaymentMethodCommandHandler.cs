using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ShopFree.Application.DTOs.PaymentMethods;
using ShopFree.Domain.Interfaces;
using ShopFree.Domain.Interfaces.Repositories;

namespace ShopFree.Application.Features.PaymentMethods.Commands.UpdatePaymentMethod;

public class UpdatePaymentMethodCommandHandler : IRequestHandler<UpdatePaymentMethodCommand, PaymentMethodDto>
{
    private readonly IPaymentMethodRepository _paymentMethodRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdatePaymentMethodCommandHandler> _logger;
    
    public UpdatePaymentMethodCommandHandler(
        IPaymentMethodRepository paymentMethodRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<UpdatePaymentMethodCommandHandler> logger)
    {
        _paymentMethodRepository = paymentMethodRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }
    
    public async Task<PaymentMethodDto> Handle(UpdatePaymentMethodCommand request, CancellationToken cancellationToken)
    {
        var paymentMethod = await _paymentMethodRepository.GetByIdAsync(request.Id, cancellationToken);
        if (paymentMethod == null)
        {
            throw new InvalidOperationException($"PaymentMethod with ID {request.Id} not found");
        }
        
        paymentMethod.Update(request.Title, request.Details);
        
        if (request.IsActive.HasValue)
        {
            if (request.IsActive.Value)
            {
                paymentMethod.Activate();
            }
            else
            {
                paymentMethod.Deactivate();
            }
        }
        
        await _paymentMethodRepository.UpdateAsync(paymentMethod, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<PaymentMethodDto>(paymentMethod);
    }
}

