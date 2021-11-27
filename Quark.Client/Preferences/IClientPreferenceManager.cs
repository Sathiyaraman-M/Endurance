
namespace Quark.Client.Preferences
{
    public interface IClientPreferenceManager
    {
        Task<ClientPreference> GetClientPreference();
        Task<MudTheme> GetCurrentThemeAsync();
        Task SetClientPreference(ClientPreference clientPreference);
        Task<bool> ToggleDarkModeAsync();
    }
}