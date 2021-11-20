﻿using Quark.Client.Extensions;
using Quark.Core.Requests.Identity;
using Quark.Core.Responses.Identity;
using Quark.Shared.Wrapper;
using System.Net.Http.Json;
using Quark.Shared.Constants;

namespace Quark.Client.Managers.Identity.Roles;

public class RoleHttpClient : IRoleHttpClient
{
    private readonly HttpClient _httpClient;

    public RoleHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IResult<string>> DeleteAsync(string id)
    {
        var response = await _httpClient.DeleteAsync($"{Routes.RolesEndpoints.Delete}/{id}");
        return await response.ToResult<string>();
    }

    public async Task<IResult<List<RoleResponse>>> GetRolesAsync()
    {
        var response = await _httpClient.GetAsync(Routes.RolesEndpoints.GetAll);
        return await response.ToResult<List<RoleResponse>>();
    }

    public async Task<IResult<string>> SaveAsync(RoleRequest role)
    {
        var response = await _httpClient.PostAsJsonAsync(Routes.RolesEndpoints.Save, role);
        return await response.ToResult<string>();
    }

    public async Task<IResult<PermissionResponse>> GetPermissionsAsync(string roleId)
    {
        var response = await _httpClient.GetAsync(Routes.RolesEndpoints.GetPermissions + roleId);
        return await response.ToResult<PermissionResponse>();
    }

    public async Task<IResult<string>> UpdatePermissionsAsync(PermissionRequest request)
    {
        var response = await _httpClient.PutAsJsonAsync(Routes.RolesEndpoints.UpdatePermissions, request);
        return await response.ToResult<string>();
    }
}