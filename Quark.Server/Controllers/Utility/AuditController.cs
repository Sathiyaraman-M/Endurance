using Quark.Core.Interfaces.Services;

namespace Quark.Server.Controllers.Utility;

[Route(Routes.AuditTrailEndpoints.GetAllTrails)]
[ApiController]
public class AuditController : ControllerBase
{
    private readonly IAuditService _auditService;
    private readonly ICurrentUserService _currentUserService;

    public AuditController(IAuditService auditService, ICurrentUserService currentUserService)
    {
        _auditService = auditService;
        _currentUserService = currentUserService;
    }

    [Authorize(Policy = Permissions.AuditTrails.ViewAll)]
    [HttpGet]
    public async Task<IActionResult> GetAllTrailsAsync()
    {
        return Ok(await _auditService.GetCurrentUserTrailsAsync());
    }

    [Authorize(Policy = Permissions.AuditTrails.View)]
    [HttpGet("current")]
    public async Task<IActionResult> GetCurrentUserTrailsAsync()
    {
        return Ok(await _auditService.GetCurrentUserTrailsAsync(_currentUserService.UserId));
    }
}