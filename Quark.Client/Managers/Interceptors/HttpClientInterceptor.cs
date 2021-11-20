using Microsoft.AspNetCore.Components;
using MudBlazor;
using Quark.Client.Managers.Identity.Authentication;
using System.Net.Http.Headers;
using Toolbelt.Blazor;

namespace Quark.Client.Managers.Interceptors;

public class HttpClientInterceptor : IHttpClientInterceptor
{
    private readonly Toolbelt.Blazor.HttpClientInterceptor _interceptor;
    private readonly IAuthenticationHttpClient _authenticationHttpClient;
    private readonly NavigationManager _navigationManager;
    private readonly ISnackbar _snackBar;

    public HttpClientInterceptor(Toolbelt.Blazor.HttpClientInterceptor interceptor, IAuthenticationHttpClient authenticationManager, NavigationManager navigationManager, ISnackbar snackBar)
    {
        _interceptor = interceptor;
        _authenticationHttpClient = authenticationManager;
        _navigationManager = navigationManager;
        _snackBar = snackBar;
    }

    public void RegisterEvent() => _interceptor.BeforeSendAsync += InterceptBeforeHttpAsync;

    public async Task InterceptBeforeHttpAsync(object sender, HttpClientInterceptorEventArgs e)
    {
        var absPath = e.Request.RequestUri.AbsolutePath;
        if (!absPath.Contains("token") && !absPath.Contains("accounts"))
        {
            try
            {
                var token = await _authenticationHttpClient.TryRefreshToken();
                if (!string.IsNullOrEmpty(token))
                {
                    _snackBar.Add("Refreshed Token.", Severity.Success);
                    e.Request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _snackBar.Add("You are Logged Out.", Severity.Error);
                await _authenticationHttpClient.Logout();
                _navigationManager.NavigateTo("/");
            }
        }
    }

    public void DisposeEvent() => _interceptor.BeforeSendAsync -= InterceptBeforeHttpAsync;
}