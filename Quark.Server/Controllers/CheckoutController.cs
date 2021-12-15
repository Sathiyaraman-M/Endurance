using Quark.Core.Features.Checkouts.Commands;
using Quark.Core.Features.Checkouts.Queries;

namespace Quark.Server.Controllers;

[Route(Routes.CheckoutEndpoints.BaseRoute)]
public class CheckoutController : BaseApiController
{
    [Authorize(Policy = Permissions.Checkouts.View)]
    [HttpGet]
    public async Task<IActionResult> GetAllAsync(int pageNumber, int pageSize, string searchString, string orderBy)
    {
        return Ok(await _mediator.Send(new GetAllCheckoutsQuery(pageNumber, pageSize, searchString, orderBy)));
    }

    [Authorize(Policy = Permissions.Checkouts.View)]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        return Ok(await _mediator.Send(new GetCheckoutByIdQuery(id)));
    }

    [Authorize(Policy = Permissions.Checkouts.View)]
    [HttpGet("checkins")]
    public async Task<IActionResult> GetCheckInsByDateAsync([FromQuery] DateTime date)
    {
        return Ok(await _mediator.Send(new GetCheckInByDateQuery(date)));
    }

    [Authorize(Policy = Permissions.Checkouts.Create)]
    [HttpPost]
    public async Task<IActionResult> CreateAsync(AddCheckoutCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    [Authorize(Policy = Permissions.Checkouts.Edit)]
    [HttpPost("extend")]
    public async Task<IActionResult> ExtendDaysAsync(ExtendCheckoutCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    [Authorize(Policy = Permissions.Checkouts.Edit)]
    [HttpPost("close")]
    public async Task<IActionResult> CheckInAsync(CheckInBookCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    [Authorize(Policy = Permissions.Checkouts.Delete)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        return Ok(await _mediator.Send(new DeleteCheckoutCommand(id)));
    }
}