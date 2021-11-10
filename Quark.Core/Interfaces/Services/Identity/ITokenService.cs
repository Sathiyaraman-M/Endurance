using Quark.Core.Requests.Identity;
using Quark.Core.Responses.Identity;
using Quark.Shared.Wrapper;

namespace Quark.Core.Interfaces.Services.Identity;

public interface ITokenService
{
    Task<Result<TokenResponse>> LoginAsync(TokenRequest model);

    Task<Result<TokenResponse>> GetRefreshTokenAsync(RefreshTokenRequest model);
}