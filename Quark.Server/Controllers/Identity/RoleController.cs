using Quark.Core.Interfaces.Services.Identity;
using Quark.Core.Requests.Identity;

namespace Quark.Server.Controllers.Identity;

[Route(Routes.RolesEndpoints.GetAll)]
[ApiController]
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [Authorize(Policy = Permissions.Roles.View)]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var roles = await _roleService.GetAllAsync();
        return Ok(roles);
    }

    [Authorize(Policy = Permissions.Roles.Create)]
    [HttpPost]
    public async Task<IActionResult> Post(RoleRequest request)
    {
        var response = await _roleService.SaveAsync(request);
        return Ok(response);
    }

    [Authorize(Policy = Permissions.Roles.Delete)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var response = await _roleService.DeleteAsync(id);
        return Ok(response);
    }

    [Authorize(Policy = Permissions.RoleClaims.View)]
    [HttpGet("permissions/{roleId}")]
    public async Task<IActionResult> GetPermissionsByRoleId([FromRoute] string roleId)
    {
        var response = await _roleService.GetAllPermissionsAsync(roleId);
        return Ok(response);
    }

    [Authorize(Policy = Permissions.RoleClaims.Edit)]
    [HttpPut("permissions/update")]
    public async Task<IActionResult> Update(PermissionRequest model)
    {
        var response = await _roleService.UpdatePermissionsAsync(model);
        return Ok(response);
    }
}