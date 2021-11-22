using Quark.Client.Extensions;
using Quark.Shared.Wrapper;
using System.Net.Http.Json;

namespace Quark.Client.HttpClients.Generic;

public class GenericHttpClient<T, TId> : IGenericHttpClient<T, TId>
{
    private readonly HttpClient _httpClient;

    public GenericHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IResult<List<T>>> GetAllAsync(string route)
    {
        var response = await _httpClient.GetAsync(route);
        return await response.ToResult<List<T>>();
    }

    public async Task<IResult<T>> GetBydIdAsync(TId id, string route)
    {
        var response = await _httpClient.GetAsync($"{route}/{id}");
        return await response.ToResult<T>();
    }

    public async Task<IResult<TId>> SaveAsync(T request, string route)
    {
        var response = await _httpClient.PostAsJsonAsync(route, request);
        return await response.ToResult<TId>();
    }

    public async Task<IResult<TId>> DeleteAsync(TId id, string route)
    {
        var response = await _httpClient.DeleteAsync($"{route}/{id}");
        return await response.ToResult<TId>();
    }
}