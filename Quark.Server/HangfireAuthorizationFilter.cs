using Hangfire.Dashboard;
using Quark.Shared.Constants.Permission;

public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        var httpContext = context.GetHttpContext();
        return httpContext.User.IsInRole(Permissions.Hangfire.View);
    }
}