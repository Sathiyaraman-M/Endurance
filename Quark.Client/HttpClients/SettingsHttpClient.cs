using Quark.Core.Configurations;

namespace Quark.Client.HttpClients;

public class SettingsHttpClient
{
    private readonly HttpClient _httpClient;

    public SettingsHttpClient(HttpClient httpClient) => _httpClient = httpClient;

    public async Task<IResult<LibrarySettings>> GetSettings()
    {
        var response = await _httpClient.GetAsync(Routes.SettingsEndpoints.Get);
        return await response.ToResult<LibrarySettings>();
    }

    public async Task<IResult> UpdateSettings(LibrarySettings librarySettings)
    {
        var response = await _httpClient.PostAsJsonAsync(Routes.SettingsEndpoints.Update, librarySettings);
        return await response.ToResult();
    }
}