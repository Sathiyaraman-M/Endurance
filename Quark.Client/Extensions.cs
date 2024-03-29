﻿using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Quark.Client.Authentication;
using Quark.Client.HttpClients;
using Quark.Client.HttpClients.Identity;
using Quark.Client.HttpClients.Masters;
using Quark.Client.Preferences;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Toolbelt.Blazor.Extensions.DependencyInjection;

namespace Quark.Client.Extensions;

internal static class WebAssemblyHostBuilderExtensions
{
    private const string ClientName = "Quark.LibraryManagementSystem.API";

    internal static WebAssemblyHostBuilder AddRootComponent(this WebAssemblyHostBuilder builder)
    {
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");
        return builder;
    }

    internal static WebAssemblyHostBuilder AddClientServices(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
        builder.Services.AddAuthorizationCore(options => RegisterPermissionClaims(options));
        builder.Services.AddMudServices();
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        builder.Services.AddBlazoredLocalStorage();
        builder.Services.AddTransient<AuthenticationHeaderHandler>();
        builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient(ClientName).EnableIntercept(sp));
        builder.Services.AddHttpClient(ClientName, client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)).AddHttpMessageHandler<AuthenticationHeaderHandler>();
        builder.Services.AddHttpClientInterceptor();
        builder.Services.AddScoped<UserAuthenticationStateProvider>();
        builder.Services.AddScoped<AuthenticationStateProvider, UserAuthenticationStateProvider>();
        builder.Services.AddSingleton<Navigation>();
        builder.Services.AddTransient<ClientPreferenceManager>();
        builder.Services.AddHttpClients();
        return builder;
    }

    internal static IServiceCollection AddHttpClients(this IServiceCollection services)
    {
        services.AddTransient<AuditHttpClient>();
        services.AddTransient<AccountHttpClient>();
        services.AddTransient<AuthenticationHttpClient>();
        services.AddTransient<BookHttpClient>();
        services.AddTransient<CheckoutHttpClient>();
        services.AddTransient<HttpClientInterceptor>();
        services.AddTransient<PatronHttpClient>();
        services.AddTransient<RoleHttpClient>();
        services.AddTransient<SettingsHttpClient>();
        services.AddTransient<UserHttpClient>();
        return services;
    }

    internal static void RegisterPermissionClaims(AuthorizationOptions options)
    {
        foreach (var prop in typeof(Permissions).GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)))
        {
            var propertyValue = prop.GetValue(null);
            if (propertyValue is not null)
            {
                options.AddPolicy(propertyValue.ToString(), policy => policy.RequireClaim(ApplicationClaimTypes.Permission, propertyValue.ToString()));
            }
        }
    }
}

internal static class ResultExtensions
{
    internal static async Task<IResult<T>> ToResult<T>(this HttpResponseMessage response)
    {
        var responseAsString = await response.Content.ReadAsStringAsync();
        var responseObject = JsonSerializer.Deserialize<Result<T>>(responseAsString, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            ReferenceHandler = ReferenceHandler.Preserve
        });
        return responseObject;
    }

    internal static async Task<IResult> ToResult(this HttpResponseMessage response)
    {
        var responseAsString = await response.Content.ReadAsStringAsync();
        var responseObject = JsonSerializer.Deserialize<Result>(responseAsString, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            ReferenceHandler = ReferenceHandler.Preserve
        });
        return responseObject;
    }

    internal static async Task<PaginatedResult<T>> ToPaginatedResult<T>(this HttpResponseMessage response)
    {
        var responseAsString = await response.Content.ReadAsStringAsync();
        var responseObject = JsonSerializer.Deserialize<PaginatedResult<T>>(responseAsString, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        return responseObject;
    }
}

internal static class ClaimsPrincipalExtensions
{
    internal static string GetEmail(this ClaimsPrincipal claimsPrincipal)
        => claimsPrincipal.FindFirstValue(ClaimTypes.Email);

    internal static string GetUserName(this ClaimsPrincipal claimsPrincipal)
        => claimsPrincipal.FindFirstValue(ClaimTypes.Name);

    internal static string GetFullName(this ClaimsPrincipal claimsPrincipal)
        => claimsPrincipal.FindFirstValue("FullName");

    internal static string GetPhoneNumber(this ClaimsPrincipal claimsPrincipal)
        => claimsPrincipal.FindFirstValue(ClaimTypes.MobilePhone);

    internal static string GetUserId(this ClaimsPrincipal claimsPrincipal)
       => claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
}