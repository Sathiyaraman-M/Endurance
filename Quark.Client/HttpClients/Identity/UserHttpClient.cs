using Quark.Core.Responses.Identity;

namespace Quark.Client.HttpClients.Identity;

public class UserHttpClient
{
    private readonly HttpClient _httpClient;

    public UserHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IResult<List<UserResponse>>> GetAllAsync()
    {
        var response = await _httpClient.GetAsync(Routes.UserEndpoints.BaseRoute);
        return await response.ToResult<List<UserResponse>>();
    }

    public async Task<IResult<UserResponse>> GetAsync(string userId)
    {
        var response = await _httpClient.GetAsync(Routes.UserEndpoints.Get(userId));
        return await response.ToResult<UserResponse>();
    }

    public async Task<IResult> RegisterUserAsync(RegisterRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync(Routes.UserEndpoints.BaseRoute, request);
        return await response.ToResult();
    }

    public async Task<IResult> ToggleUserStatusAsync(ToggleUserStatusRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync(Routes.UserEndpoints.ToggleActivation, request);
        return await response.ToResult();
    }

    public async Task<IResult<UserRolesResponse>> GetRolesAsync(string userId)
    {
        var response = await _httpClient.GetAsync(Routes.UserEndpoints.GetUserRoles(userId));
        return await response.ToResult<UserRolesResponse>();
    }

    public async Task<IResult> UpdateRolesAsync(UpdateUserRolesRequest request)
    {
        var response = await _httpClient.PutAsJsonAsync(Routes.UserEndpoints.GetUserRoles(request.UserId), request);
        return await response.ToResult<UserRolesResponse>();
    }

    public async Task<IResult> ForgotPasswordAsync(ForgotPasswordRequest model)
    {
        var response = await _httpClient.PostAsJsonAsync(Routes.UserEndpoints.ForgotPassword, model);
        return await response.ToResult();
    }

    public async Task<IResult> ResetPasswordAsync(ResetPasswordRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync(Routes.UserEndpoints.ResetPassword, request);
        return await response.ToResult();
    }

    public async Task<string> ExportToExcelAsync(string searchString = "")
    {
        var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
            ? Routes.UserEndpoints.Export
            : Routes.UserEndpoints.ExportFiltered(searchString));
        var data = await response.Content.ReadAsStringAsync();
        return data;
    }
}