using Quark.Client.Extensions;
using Quark.Core.Requests.Identity;
using Quark.Shared.Constants;
using Quark.Shared.Wrapper;
using System.Net.Http.Json;

namespace Quark.Client.Managers.Identity.Account;

public class AccountManager : IAccountManager
{
    private readonly HttpClient _httpClient;

    public AccountManager(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IResult> ChangePasswordAsync(ChangePasswordRequest model)
    {
        var response = await _httpClient.PutAsJsonAsync(Routes.AccountEndpoints.ChangePassword, model);
        return await response.ToResult();
    }

    public async Task<IResult> UpdateProfileAsync(UpdateProfileRequest model)
    {
        var response = await _httpClient.PutAsJsonAsync(Routes.AccountEndpoints.UpdateProfile, model);
        return await response.ToResult();
    }

    public async Task<IResult<string>> GetProfilePictureAsync(string userId)
    {
        var response = await _httpClient.GetAsync($"{Routes.AccountEndpoints.ProfilePicture}/{userId}");
        return await response.ToResult<string>();
    }

    public async Task<IResult<string>> UpdateProfilePictureAsync(UpdateProfilePictureRequest request, string userId)
    {
        var response = await _httpClient.PostAsJsonAsync($"{Routes.AccountEndpoints.ProfilePicture}/{userId}", request);
        return await response.ToResult<string>();
    }
}