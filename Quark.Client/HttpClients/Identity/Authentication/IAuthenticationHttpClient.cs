using Quark.Core.Requests.Identity;
using Quark.Shared.Wrapper;
using System.Security.Claims;

namespace Quark.Client.HttpClients.Identity.Authentication;

public interface IAuthenticationHttpClient
{
    Task<IResult> Login(TokenRequest model);

    Task<IResult> Logout();

    Task<string> RefreshToken();

    Task<string> TryRefreshToken();

    Task<string> TryForceRefreshToken();

    Task<ClaimsPrincipal> CurrentUser();
}