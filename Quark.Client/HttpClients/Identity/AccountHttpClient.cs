namespace Quark.Client.HttpClients.Identity;

public class AccountHttpClient
{
    private readonly HttpClient _httpClient;

    public AccountHttpClient(HttpClient httpClient)
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
        var response = await _httpClient.GetAsync($"{Routes.AccountEndpoints.ProfilePictureShort}/{userId}");
        return await response.ToResult<string>();
    }

    public async Task<IResult<string>> UpdateProfilePictureAsync(UpdateProfilePictureRequest request, string userId)
    {
        var response = await _httpClient.PostAsJsonAsync($"{Routes.AccountEndpoints.ProfilePictureShort}/{userId}", request);
        return await response.ToResult<string>();
    }
}