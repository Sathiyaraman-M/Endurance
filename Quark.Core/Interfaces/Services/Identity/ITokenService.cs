namespace Quark.Core.Interfaces.Services.Identity;

public interface ITokenService
{
    Task<Result<TokenResponse>> LoginAsync(TokenRequest model);

    Task<Result<TokenResponse>> GetRefreshTokenAsync(RefreshTokenRequest model);
}