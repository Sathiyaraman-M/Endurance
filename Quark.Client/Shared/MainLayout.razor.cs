using System.Security.Claims;

namespace Quark.Client.Shared;

public partial class MainLayout
{
    private bool _drawerOpen = true;

    private ClaimsPrincipal User;
    private string Name;
    private string Designation;

    protected override async Task OnInitializedAsync()
    {
        User = await authStateProvider.GetAuthenticationStateProviderUserAsync();
        if (User.Identity.IsAuthenticated)
        {
            Name = User.Claims.FirstOrDefault(x => x.Type == "FullName")?.Value;
            Designation = User.Claims.FirstOrDefault(x => x.Type == "Designation")?.Value;
        }
    }

    private async Task Logout()
    {
        if (await dialogService.ShowMessageBox("Confirm Logout", "Are you sure want to logout?", yesText: "Log out", cancelText: "Cancel") == true)
        {
            await authenticationManager.Logout();
            navigationManager.NavigateTo("/");
        }
    }

    public void Dispose()
    {
        httpInterceptorManager.DisposeEvent();
    }
}