namespace Quark.Client.Pages.Content;

public partial class Index
{
    private DashboardResponse DashboardData;

    protected override async Task OnInitializedAsync()
    {
        var response = await httpClient.GetAsync(Routes.DashboardRoute);
        var result = await response.ToResult<DashboardResponse>();
        if (result.Succeeded)
        {
            DashboardData = result.Data;
        }
    }
}