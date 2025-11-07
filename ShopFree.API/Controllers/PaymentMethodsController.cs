using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopFree.Application.Features.PaymentMethods.Commands.CreatePaymentMethod;
using ShopFree.Application.Features.PaymentMethods.Commands.UpdatePaymentMethod;
using ShopFree.Application.Features.PaymentMethods.Queries.GetPaymentMethodsByStoreId;

namespace ShopFree.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PaymentMethodsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PaymentMethodsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("store/{storeId}")]
    public async Task<IActionResult> GetPaymentMethodsByStore(int storeId, [FromQuery] bool activeOnly = false)
    {
        var query = new GetPaymentMethodsByStoreIdQuery { StoreId = storeId, ActiveOnly = activeOnly };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePaymentMethod([FromBody] CreatePaymentMethodCommand command)
    {
        try
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetPaymentMethodsByStore), new { storeId = result.StoreId }, result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePaymentMethod(int id, [FromBody] UpdatePaymentMethodCommand command)
    {
        try
        {
            command.Id = id;
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}

