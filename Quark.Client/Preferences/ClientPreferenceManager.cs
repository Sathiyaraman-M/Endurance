using Blazored.LocalStorage;

namespace Quark.Client.Preferences;

public class ClientPreferenceManager : IClientPreferenceManager
{
    private readonly ILocalStorageService _localStorageService;

    public ClientPreferenceManager(ILocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;
    }
    public async Task<bool> ToggleDarkModeAsync()
    {
        var preference = await GetClientPreference();
        if (preference != null)
        {
            preference.IsDarkMode = !preference.IsDarkMode;
            await SetClientPreference(preference);
            return !preference.IsDarkMode;
        }

        return false;
    }

    public async Task<MudTheme> GetCurrentThemeAsync()
    {
        var preference = await GetClientPreference();
        if (preference != null)
        {
            if (preference.IsDarkMode == true) return AppThemes.DarkTheme;
        }
        return AppThemes.DefaultTheme;
    }

    public async Task<ClientPreference> GetClientPreference()
    {
        return await _localStorageService.GetItemAsync<ClientPreference>(StorageConstants.ClientPreference) ?? new ClientPreference();
    }

    public async Task SetClientPreference(ClientPreference clientPreference)
    {
        await _localStorageService.SetItemAsync(StorageConstants.ClientPreference, clientPreference);
    }
}