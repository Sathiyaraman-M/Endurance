namespace Quark.Client.Shared;

public partial class MainLayout
{
    private bool _drawerOpen = true;

    private ClaimsPrincipal User;
    private string CurrentUserId { get; set; }
    private string ImageDataUrl { get; set; }
    private string UserName { get; set; }
    private string FullName { get; set; }
    private string Designation { get; set; }
    private string Email { get; set; }
    private char FirstLetterOfName { get; set; }

    protected override async Task OnInitializedAsync()
    {
        _currentTheme = _defaultTheme;
        User = await authStateProvider.GetAuthenticationStateProviderUserAsync();
        if (User == null) return;
        if (User.Identity.IsAuthenticated)
        {
            UserName = User.GetUserName();
            FullName = User.GetFullName();
            Designation = User.GetDesignation();
            CurrentUserId = User.GetUserId();
            if (UserName.Length > 0)
            {
                FirstLetterOfName = UserName[0];
            }
            Email = User.GetEmail();
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

    private void DarkMode() => _currentTheme = _currentTheme == _defaultTheme ? _darkTheme : _defaultTheme;

    private MudTheme _currentTheme = new();
    private readonly MudTheme _defaultTheme =
        new()
        {
            Palette = new Palette()
            {
                Black = "#272c34"
            }
        };
    private readonly MudTheme _darkTheme =
        new()
        {
            Palette = new Palette()
            {
                Primary = "#776be7",
                Black = "#27272f",
                Background = "#32333d",
                BackgroundGrey = "#27272f",
                Surface = "#373740",
                DrawerBackground = "#27272f",
                DrawerText = "rgba(255,255,255, 0.50)",
                DrawerIcon = "rgba(255,255,255, 0.50)",
                AppbarBackground = "#27272f",
                AppbarText = "rgba(255,255,255, 0.70)",
                TextPrimary = "rgba(255,255,255, 0.70)",
                TextSecondary = "rgba(255,255,255, 0.50)",
                ActionDefault = "#adadb1",
                ActionDisabled = "rgba(255,255,255, 0.26)",
                ActionDisabledBackground = "rgba(255,255,255, 0.12)",
                Divider = "rgba(255,255,255, 0.12)",
                DividerLight = "rgba(255,255,255, 0.06)",
                TableLines = "rgba(255,255,255, 0.12)",
                LinesDefault = "rgba(255,255,255, 0.12)",
                LinesInputs = "rgba(255,255,255, 0.3)",
                TextDisabled = "rgba(255,255,255, 0.2)",
                Info = "#3299ff",
                Success = "#0bba83",
                Warning = "#ffa800",
                Error = "#f64e62",
                Dark = "#27272f"
            }
        };

    public void Dispose()
    {
        httpInterceptorManager.DisposeEvent();
    }
}