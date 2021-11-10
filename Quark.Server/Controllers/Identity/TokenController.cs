using Microsoft.AspNetCore.Mvc;
using Quark.Core.Interfaces.Services.Identity;
using Quark.Core.Requests.Identity;

namespace Quark.Server.Controllers.Identity;

[Route("api/identity/token")]
[ApiController]
public class TokenController : ControllerBase
{
    private readonly ITokenService _tokenService;

    public TokenController(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    /// <summary>
    /// Get Token (Email, Password)
    /// </summary>
    /// <param name="model"></param>
    /// <returns>Status 200 OK</returns>
    [HttpPost]
    public async Task<ActionResult> Get(TokenRequest model)
    {
        var response = await _tokenService.LoginAsync(model);
        return Ok(response);
    }

    /// <summary>
    /// Refresh Token
    /// </summary>
    /// <param name="model"></param>
    /// <returns>Status 200 OK</returns>
    [HttpPost("refresh")]
    public async Task<ActionResult> Refresh([FromBody] RefreshTokenRequest model)
    {
        var response = await _tokenService.GetRefreshTokenAsync(model);
        return Ok(response);
    }
}