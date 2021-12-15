using Quark.Core.Features.Checkouts.Commands;

namespace Quark.Client.Pages.Content;

public partial class Dashboard
{
    private DashboardResponse DashboardData { get; set; }
    private AddCheckoutCommand Checkout { get; set; } = new();
    private MudDatePicker _picker1;
    private MudDatePicker _picker2;

    private ClaimsPrincipal User;
    private bool _canViewCheckout;
    private bool _canCreateCheckout;
    private bool _canViewDashboard;

    private List<CheckoutResponse> _checkInsToday = new();

    protected override async Task OnInitializedAsync()
    {
        User = await authStateProvider.GetAuthenticationStateProviderUserAsync();
        _canViewCheckout = (await authorizationService.AuthorizeAsync(User, Permissions.Checkouts.View)).Succeeded;
        _canViewDashboard = (await authorizationService.AuthorizeAsync(User, Permissions.Dashboard.View)).Succeeded;
        _canCreateCheckout = (await authorizationService.AuthorizeAsync(User, Permissions.Checkouts.Create)).Succeeded;
        var response = await httpClient.GetAsync(Routes.DashboardRoute);
        var result = await response.ToResult<DashboardResponse>();
        if (result.Succeeded)
        {
            DashboardData = result.Data;
        }
        var checkIns = await _checkoutHttpClient.GetCheckInByDateAsync(DateTime.Today);
        if (checkIns.Succeeded) _checkInsToday = checkIns.Data;
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