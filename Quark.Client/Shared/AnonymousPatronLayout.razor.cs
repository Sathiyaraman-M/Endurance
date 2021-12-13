using Quark.Client.Preferences;

namespace Quark.Client.Shared;

public partial class AnonymousPatronLayout
{
    protected override async Task OnInitializedAsync()
    {
        _currentTheme = await clientPreferenceManager.GetCurrentThemeAsync();
        await base.OnInitializedAsync();
    }

    private MudTheme _currentTheme = new();

    private async Task DarkMode()
    {
        _currentTheme = _currentTheme != AppThemes.DefaultTheme ? AppThemes.DefaultTheme : AppThemes.DarkTheme;
        await clientPreferenceManager.ToggleDarkModeAsync();
    }
}