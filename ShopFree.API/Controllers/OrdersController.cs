using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopFree.Application.Features.Orders.Commands.CreateOrder;
using ShopFree.Application.Features.Orders.Commands.UpdateOrderStatus;
using ShopFree.Application.Features.Orders.Queries.GetOrderById;
using ShopFree.Application.Features.Orders.Queries.GetOrderByOrderNumber;
using ShopFree.Application.Features.Orders.Queries.GetOrdersByStoreId;

namespace ShopFree.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrder(int id)
    {
        var query = new GetOrderByIdQuery { Id = id };
        var result = await _mediator.Send(query);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet("number/{orderNumber}")]
    public async Task<IActionResult> GetOrderByNumber(string orderNumber)
    {
        var query = new GetOrderByOrderNumberQuery { OrderNumber = orderNumber };
        var result = await _mediator.Send(query);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet("store/{storeId}")]
    public async Task<IActionResult> GetOrdersByStore(int storeId)
    {
        var query = new GetOrdersByStoreIdQuery { StoreId = storeId };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand command)
    {
        try
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetOrder), new { id = result.Id }, result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] UpdateOrderStatusCommand command)
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

