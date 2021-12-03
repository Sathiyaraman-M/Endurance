namespace Quark.Client.Shared;

public partial class AuthLayout
{
    protected override async Task OnInitializedAsync()
    {
        _currentTheme = await clientPreferenceManager.GetCurrentThemeAsync();
        await base.OnInitializedAsync();
    }

    private MudTheme _currentTheme = new();
}