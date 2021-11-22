using Blazored.LocalStorage;
using System.Net.Http.Headers;

namespace Quark.Client.Authentication;

public class AuthenticationHeaderHandler : DelegatingHandler
{
    private readonly ILocalStorageService localStorageService;

    public AuthenticationHeaderHandler(ILocalStorageService localStorage) => localStorageService = localStorage;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.Headers.Authorization?.Scheme != "Bearer")
        {
            var savedToken = await localStorageService.GetItemAsync<string>(StorageConstants.AuthToken);
            if (!string.IsNullOrWhiteSpace(savedToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", savedToken);
            }
        }
        return await base.SendAsync(request, cancellationToken);
    }
}