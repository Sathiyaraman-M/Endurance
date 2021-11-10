using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quark.Core.Interfaces.Services;
using Quark.Core.Interfaces.Services.Identity;
using Quark.Core.Requests.Identity;
using Quark.Shared.Constants;

namespace Quark.Server.Controllers.Identity;

[Authorize]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly ICurrentUserService _currentUserService;

    public AccountController(IAccountService accountService, ICurrentUserService currentUserService)
    {
        _accountService = accountService;
        _currentUserService = currentUserService;
    }

    [HttpPut(Routes.AccountEndpoints.UpdateProfile)]
    public async Task<ActionResult> UpdateProfile(UpdateProfileRequest model)
    {
        return Ok(await _accountService.UpdateProfileAsync(model, _currentUserService.UserId));
    }

    [HttpPut(Routes.AccountEndpoints.ChangePassword)]
    public async Task<ActionResult> ChangePassword(ChangePasswordRequest model)
    {
        return Ok(await _accountService.ChangePasswordAsync(model, _currentUserService.UserId));
    }

    [HttpGet(Routes.AccountEndpoints.ProfilePicture)]
    [ResponseCache(NoStore = false, Location = ResponseCacheLocation.Client, Duration = 60)]
    public async Task<ActionResult> GetProfilePictureAsync(string userId)
    {
        return Ok(await _accountService.GetProfilePictureAsync(userId));
    }

    [HttpPost(Routes.AccountEndpoints.ProfilePicture)]
    public async Task<ActionResult> UpdateProfilePictureAsync(UpdateProfilePictureRequest model)
    {
        return Ok(await _accountService.UpdateProfilePictureAsync(model, _currentUserService.UserId));
    }
}