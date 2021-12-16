using Quark.Core.Features.Dashboard;

namespace Quark.Server.Controllers;

[Route(Routes.DashboardRoute)]
public class DashboardController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetDataAsync()
    {
        var response = await _mediator.Send(new DashboardQuery());
        return Ok(response);
    }
}