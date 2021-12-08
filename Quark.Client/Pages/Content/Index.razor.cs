using Quark.Core.Features.Checkouts.Commands;

namespace Quark.Client.Pages.Content;

public partial class Index
{
    private DashboardResponse DashboardData;
    private AddCheckoutCommand Checkout = new();
    private MudDatePicker picker1;
    private MudDatePicker picker2;

    private ClaimsPrincipal User;
    private bool canViewCheckout;
    private bool canCreateCheckout;
    private bool canViewDashboard;

    protected override async Task OnInitializedAsync()
    {
        await Task.Delay(1000);
        User = await authStateProvider.GetAuthenticationStateProviderUserAsync();
        canViewCheckout = (await authorizationService.AuthorizeAsync(User, Permissions.Checkouts.View)).Succeeded;
        canViewDashboard = (await authorizationService.AuthorizeAsync(User, Permissions.Dashboard.View)).Succeeded;
        canCreateCheckout = (await authorizationService.AuthorizeAsync(User, Permissions.Checkouts.Create)).Succeeded;
        var response = await (await _httpClient.GetAsync(Routes.DashboardRoute)).ToResult<DashboardResponse>();
        if (response.Succeeded)
        {
            DashboardData = response.Data;
        }
    }

    private void ResetModel()
    {
        Checkout.BookBarcode = "";
        Checkout.PatronRegisterId = "";
        Checkout.CheckedOutSince = DateTime.Now;
        Checkout.ExpectedCheckInDate = DateTime.Now.AddDays(15);
    }

    private async Task AddCheckoutAsync()
    {
        var response = await _checkoutHttpClient.AddCheckoutAsync(Checkout);
        foreach (var message in response.Messages)
        {
            if (response.Succeeded)
            {
                snackbar.Add(message, Severity.Success);
            }
            else
            {
                snackbar.Add(message, Severity.Error);
            }
        }
    }
}