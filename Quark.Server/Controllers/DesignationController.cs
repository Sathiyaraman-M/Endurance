using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quark.Core.Features.Designations.Commands;
using Quark.Core.Features.Designations.Queries;
using Quark.Server.Controllers.Utility;
using Quark.Shared.Constants;
using Quark.Shared.Constants.Permission;

namespace Quark.Server.Controllers;

[Route(Routes.DesignationEndpoints.BaseRoute)]
public class DesignationController : BaseApiController
{
    [Authorize(Policy = Permissions.Designations.View)]
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        return Ok(await _mediator.Send(new GetAllDesignationsQuery()));
    }

    [Authorize(Policy = Permissions.Designations.Create)]
    [HttpPost]
    public async Task<IActionResult> PostAsync(AddEditDesignationCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    [Authorize(Policy = Permissions.Designations.Delete)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        return Ok(await _mediator.Send(new DeleteDesignationCommand { Id = id }));
    }
}