using Quark.Client.Preferences;

namespace Quark.Client.Shared;

public partial class MainLayout
{
    private bool _drawerOpen = true;

    private ClaimsPrincipal _user;
    private string CurrentUserId { get; set; }
    private string ImageDataUrl { get; set; }
    private string UserName { get; set; }
    private string FullName { get; set; }
    private string Designation { get; set; }
    private string Email { get; set; }
    private char FirstLetterOfName { get; set; }

    protected override async Task OnInitializedAsync()
    {
        _currentTheme = await clientPreferenceManager.GetCurrentThemeAsync();
        _drawerOpen = (await clientPreferenceManager.GetClientPreference()).IsDrawerOpen;
        _user = await authStateProvider.GetAuthenticationStateProviderUserAsync();
        if (_user == null) return;
        if (_user.Identity.IsAuthenticated)
        {
            UserName = _user.GetUserName();
            FullName = _user.GetFullName();
            Designation = _user.GetDesignation();
            CurrentUserId = _user.GetUserId();
            if (UserName.Length > 0)
            {
                FirstLetterOfName = UserName[0];
            }
            Email = _user.GetEmail();
            var imageResponse = await accountHttpClient.GetProfilePictureAsync(CurrentUserId);
            if (imageResponse.Succeeded)
            {
                ImageDataUrl = imageResponse.Data;
            }

            var currentUserResult = await userHttpClient.GetAsync(CurrentUserId);
            if (!currentUserResult.Succeeded || currentUserResult.Data == null)
            {
                snackbar.Add("You are logged out because the user with your Token has been deleted.", Severity.Error);
                await authenticationHttpClient.Logout();
            }
        }
    }

    private async Task Logout()
    {
        if (await dialogService.ShowMessageBox("Confirm Logout", "Are you sure want to logout?", yesText: "Log out", cancelText: "Cancel") == true)
        {
            await authenticationHttpClient.Logout();
            navigationManager.NavigateTo("/");
        }
    }

    private void OnSwipe(SwipeDirection direction)
    {
        if (direction == SwipeDirection.LeftToRight && !_drawerOpen)
        {
            _drawerOpen = true;
            StateHasChanged();
        }
        else if (direction == SwipeDirection.RightToLeft && _drawerOpen)
        {
            _drawerOpen = false;
            StateHasChanged();
        }
    }

    private async Task ToggleDrawer()
    {
        _drawerOpen = !_drawerOpen;
        var preference = await clientPreferenceManager.GetClientPreference();
        preference.IsDrawerOpen = _drawerOpen;
        await clientPreferenceManager.SetClientPreference(preference);
    }

    private async Task DarkMode()
    {
        _currentTheme = _currentTheme != AppThemes.DefaultTheme ? AppThemes.DefaultTheme : AppThemes.DarkTheme;
        await clientPreferenceManager.ToggleDarkModeAsync();
    }

    private MudTheme _currentTheme = new();

    public void Dispose()
    {
        httpInterceptorManager.DisposeEvent();
    }
}