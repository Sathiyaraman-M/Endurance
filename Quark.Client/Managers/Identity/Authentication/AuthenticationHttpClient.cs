using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Quark.Client.Authentication;
using Quark.Client.Extensions;
using Quark.Core.Requests.Identity;
using Quark.Core.Responses.Identity;
using Quark.Shared.Constants;
using Quark.Shared.Wrapper;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;

namespace Quark.Client.Managers.Identity.Authentication;

public class AuthenticationHttpClient : IAuthenticationHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorageService;
    private readonly AuthenticationStateProvider _authenticationStateProvider;

    public AuthenticationHttpClient(HttpClient httpClient, ILocalStorageService localStorageService, AuthenticationStateProvider authenticationStateProvider)
    {
        _httpClient = httpClient;
        _localStorageService = localStorageService;
        _authenticationStateProvider = authenticationStateProvider;
    }

    public async Task<ClaimsPrincipal> CurrentUser()
    {
        var state = await _authenticationStateProvider.GetAuthenticationStateAsync();
        return state.User;
    }

    public async Task<IResult> Login(TokenRequest model)
    {
        var response = await _httpClient.PostAsJsonAsync(Routes.TokenEndpoints.Get, model);
        var result = await response.ToResult<TokenResponse>();
        if (result.Succeeded)
        {
            var token = result.Data.Token;
            var refreshToken = result.Data.RefreshToken;
            var userImageURL = result.Data.UserImageURL;
            await _localStorageService.SetItemAsync(StorageConstants.AuthToken, token);
            await _localStorageService.SetItemAsync(StorageConstants.RefreshToken, refreshToken);
            if (!string.IsNullOrEmpty(userImageURL))
            {
                await _localStorageService.SetItemAsync(StorageConstants.UserImageURL, userImageURL);
            }
            ((UserAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(model.UserName);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return await Result.SuccessAsync();
        }
        else
        {
            return await Result.FailAsync(result.Messages);
        }
    }

    public async Task<IResult> Logout()
    {
        await _localStorageService.RemoveItemAsync(StorageConstants.AuthToken);
        await _localStorageService.RemoveItemAsync(StorageConstants.RefreshToken);
        await _localStorageService.RemoveItemAsync(StorageConstants.UserImageURL);
        ((UserAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
        _httpClient.DefaultRequestHeaders.Authorization = null;
        return await Result.SuccessAsync();
    }

    public async Task<string> RefreshToken()
    {
        var token = await _localStorageService.GetItemAsync<string>(StorageConstants.AuthToken);
        var refreshToken = await _localStorageService.GetItemAsync<string>(StorageConstants.RefreshToken);

        var response = await _httpClient.PostAsJsonAsync(Routes.TokenEndpoints.Refresh, new RefreshTokenRequest { Token = token, RefreshToken = refreshToken });

        var result = await response.ToResult<TokenResponse>();

        if (!result.Succeeded)
        {
            throw new ApplicationException("Something went wrong during the refresh token action");
        }

        token = result.Data.Token;
        refreshToken = result.Data.RefreshToken;
        await _localStorageService.SetItemAsync(StorageConstants.AuthToken, token);
        await _localStorageService.SetItemAsync(StorageConstants.RefreshToken, refreshToken);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return token;
    }

    public async Task<string> TryForceRefreshToken()
    {
        var availableToken = await _localStorageService.GetItemAsync<string>(StorageConstants.RefreshToken);
        if (string.IsNullOrEmpty(availableToken)) return string.Empty;
        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;
        var exp = user.FindFirst(c => c.Type.Equals("exp"))?.Value;
        var expTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(exp));
        var diff = expTime - DateTime.Now;
        if (diff.TotalMinutes <= 1)
            return await RefreshToken();
        return string.Empty;
    }

    public async Task<string> TryRefreshToken()
    {
        return await RefreshToken();
    }
}