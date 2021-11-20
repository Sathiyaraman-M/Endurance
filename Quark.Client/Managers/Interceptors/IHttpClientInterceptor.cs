using Toolbelt.Blazor;

namespace Quark.Client.Managers.Interceptors;

public interface IHttpClientInterceptor
{
    void RegisterEvent();

    Task InterceptBeforeHttpAsync(object sender, HttpClientInterceptorEventArgs e);

    void DisposeEvent();
}