using Quark.Core.Features.Dashboard;

namespace Quark.Server.Controllers;

[Route(Routes.DashboardRoute)]
public class DashboardController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetDataAsync()
    {
        return Ok(await _mediator.Send(new DashboardQuery()));
    }
}