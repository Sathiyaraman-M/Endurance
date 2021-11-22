using MudBlazor;
using Quark.Client.Extensions;
using System.Security.Claims;

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

    public void Dispose()
    {
        httpInterceptorManager.DisposeEvent();
    }
}