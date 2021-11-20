using Toolbelt.Blazor;

namespace Quark.Client.HttpClients.Interceptors;

public interface IHttpClientInterceptor
{
    void RegisterEvent();

    Task InterceptBeforeHttpAsync(object sender, HttpClientInterceptorEventArgs e);

    void DisposeEvent();
}