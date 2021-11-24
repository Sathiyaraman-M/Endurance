using Quark.Core.Interfaces.Services.Identity;
using Quark.Core.Requests.Identity;

namespace Quark.Server.Controllers.Identity;

[Authorize]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [Authorize(Policy = Permissions.Users.View)]
    [HttpGet(Routes.UserEndpoints.BaseRoute)]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userService.GetAllAsync();
        return Ok(users);
    }

    [HttpGet(Routes.UserEndpoints.GetById)]
    public async Task<IActionResult> GetById(string id)
    {
        var user = await _userService.GetAsync(id);
        return Ok(user);
    }

    [Authorize(Policy = Permissions.Users.View)]
    [HttpGet(Routes.UserEndpoints.Roles)]
    public async Task<IActionResult> GetRolesAsync(string id)
    {
        var userRoles = await _userService.GetRolesAsync(id);
        return Ok(userRoles);
    }

    [Authorize(Policy = Permissions.Users.Edit)]
    [HttpPut(Routes.UserEndpoints.Roles)]
    public async Task<IActionResult> UpdateRolesAsync(UpdateUserRolesRequest request)
    {
        return Ok(await _userService.UpdateRolesAsync(request));
    }

    [AllowAnonymous]
    [HttpPost(Routes.UserEndpoints.BaseRoute)]
    public async Task<IActionResult> RegisterAsync(RegisterRequest request)
    {
        var origin = Request.Headers["origin"];
        return Ok(await _userService.RegisterAsync(request, origin));
    }

    [HttpGet(Routes.UserEndpoints.ConfirmEmail)]
    [AllowAnonymous]
    public async Task<IActionResult> ConfirmEmailAsync([FromQuery] string userId, [FromQuery] string code)
    {
        return Ok(await _userService.ConfirmEmailAsync(userId, code));
    }
}