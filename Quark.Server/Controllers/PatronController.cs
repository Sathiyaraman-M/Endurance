﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quark.Core.Features.Patrons.Commands;
using Quark.Core.Features.Patrons.Queries;
using Quark.Server.Controllers.Utility;
using Quark.Shared.Constants;
using Quark.Shared.Constants.Permission;

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
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        return Ok(await _mediator.Send(new GetPatronByIdQuery(id)));
    }

    [Authorize(Policy = Permissions.Patrons.Create)]
    [HttpPost]
    public async Task<IActionResult> PostAsync(AddEditPatronCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    [Authorize(Policy = Permissions.Patrons.Delete)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        return Ok(await _mediator.Send(new DeletePatronCommand { Id = id }));
    }
}