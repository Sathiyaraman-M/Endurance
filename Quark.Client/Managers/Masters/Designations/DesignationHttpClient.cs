﻿using Quark.Client.Extensions;
using Quark.Core.Features.Designations.Commands;
using Quark.Core.Responses;
using Quark.Shared.Constants;
using Quark.Shared.Wrapper;
using System.Net.Http.Json;

namespace Quark.Client.Managers.Masters.Designations;

public class DesignationHttpClient : IDesignationHttpClient
{
    private readonly HttpClient _httpClient;

    public DesignationHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IResult<List<DesignationResponse>>> GetAllAsync()
    {
        var response = await _httpClient.GetAsync(Routes.DesignationEndpoints.BaseRoute);
        return await response.ToResult<List<DesignationResponse>>();
    }

    public async Task<IResult<int>> SaveAsync(AddEditDesignationCommand request)
    {
        var response = await _httpClient.PostAsJsonAsync(Routes.DesignationEndpoints.BaseRoute, request);
        return await response.ToResult<int>();
    }

    public async Task<IResult<int>> DeleteAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"{Routes.DesignationEndpoints.BaseRoute}/{id}");
        return await response.ToResult<int>();
    }

    public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
    {
        var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString) 
            ? Routes.DesignationEndpoints.Export 
            : Routes.DesignationEndpoints.ExportFiltered(searchString));
        return await response.ToResult<string>();
    }
}