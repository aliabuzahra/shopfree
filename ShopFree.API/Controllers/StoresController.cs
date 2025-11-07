using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopFree.Application.Features.Stores.Commands.CreateStore;
using ShopFree.Application.Features.Stores.Commands.UpdateStore;
using ShopFree.Application.Features.Stores.Queries.GetStoreById;
using ShopFree.Application.Features.Stores.Queries.GetStoresByUserId;

namespace ShopFree.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StoresController : ControllerBase
{
    private readonly IMediator _mediator;

    public StoresController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetStore(int id)
    {
        var query = new GetStoreByIdQuery { Id = id };
        var result = await _mediator.Send(query);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetStoresByUser(int userId)
    {
        var query = new GetStoresByUserIdQuery { UserId = userId };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("my-stores")]
    public async Task<IActionResult> GetMyStores()
    {
        // Get user ID from JWT claims
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
        {
            return Unauthorized();
        }

        var query = new GetStoresByUserIdQuery { UserId = userId };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateStore([FromBody] CreateStoreCommand command)
    {
        try
        {
            // Get user ID from JWT claims
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized();
            }

            command.UserId = userId;
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetStore), new { id = result.Id }, result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStore(int id, [FromBody] UpdateStoreCommand command)
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

