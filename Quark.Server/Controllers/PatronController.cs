using Quark.Core.Features.Patrons.Commands;
using Quark.Core.Features.Patrons.Queries;

namespace Quark.Server.Controllers;

[Route(Routes.PatronEndpoints.BaseRoute)]
public class PatronController : BaseApiController
{
    [Authorize(Policy = Permissions.Patrons.View)]
    [HttpGet]
    public async Task<IActionResult> GetAllAsync(int pageNumber, int pageSize, string searchString, string orderBy = null)
    {
        return Ok(await _mediator.Send(new GetAllPatronsQuery(pageNumber, pageSize, searchString, orderBy)));
    }

    [Authorize(Policy = Permissions.Patrons.View)]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        return Ok(await _mediator.Send(new GetPatronByIdQuery(id)));
    }

    [Authorize(Policy = Permissions.Patrons.Create)]
    [HttpPost]
    public async Task<IActionResult> PostAsync(AddEditPatronCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    [Authorize(Policy = Permissions.Patrons.Export)]
    [HttpGet("export")]
    public async Task<IActionResult> ExportToExcelAsync(string searchString = "")
    {
        return Ok(await _mediator.Send(new ExportPatronsQuery(searchString)));
    }

    [Authorize(Policy = Permissions.Patrons.Delete)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        return Ok(await _mediator.Send(new DeletePatronCommand { Id = id }));
    }
}