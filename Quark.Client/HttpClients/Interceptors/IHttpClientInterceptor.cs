namespace Quark.Client.HttpClients.Interceptors;

public interface IHttpClientInterceptor
{
    void RegisterEvent();

    void DisposeEvent();
}