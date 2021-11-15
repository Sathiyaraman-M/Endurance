using Blazored.LocalStorage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Quark.Client.Authentication;
using Quark.Client.Managers.Audit;
using Quark.Client.Managers.Generic;
using Quark.Client.Managers.Identity.Account;
using Quark.Client.Managers.Identity.Authentication;
using Quark.Client.Managers.Identity.Roles;
using Quark.Client.Managers.Identity.Users;
using Quark.Client.Managers.Interceptors;
using Quark.Client.Managers.Masters.Books;
using Quark.Client.Managers.Masters.Checkouts;
using Quark.Client.Managers.Masters.Designations;
using Quark.Client.Managers.Masters.Patrons;
using Quark.Shared.Constants.Permission;
using System.Reflection;
using Toolbelt.Blazor.Extensions.DependencyInjection;

namespace Quark.Client.Extensions;

internal static class WebAssemblyHostBuilderExtensions
{
    private const string ClientName = "Quark.LibraryManagementSystem.API";

    internal static WebAssemblyHostBuilder AddRootComponent(this WebAssemblyHostBuilder builder)
    {
        builder.RootComponents.Add<App>("#app");
        return builder;
    }

    internal static WebAssemblyHostBuilder AddClientServices(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
        builder.Services.AddAuthorizationCore(options => RegisterPermissionClaims(options));
        builder.Services.AddMudServices();
        builder.Services.AddBlazoredLocalStorage();
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        builder.Services.AddTransient<AuthenticationHeaderHandler>();
        builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient(ClientName).EnableIntercept(sp));
        builder.Services.AddHttpClient(ClientName, client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)).AddHttpMessageHandler<AuthenticationHeaderHandler>();
        builder.Services.AddHttpClientInterceptor();
        builder.Services.AddScoped<UserAuthenticationStateProvider>();
        builder.Services.AddScoped<AuthenticationStateProvider, UserAuthenticationStateProvider>();
        builder.Services.AddSingleton<Navigation>();
        builder.Services.AddManagers();
        return builder;
    }

    internal static IServiceCollection AddManagers(this IServiceCollection services)
    {
        services.AddTransient<IAuditManager, AuditManager>();
        services.AddTransient<IAccountManager, AccountManager>();
        services.AddTransient<IAuthenticationManager, AuthenticationManager>();
        services.AddTransient<IBookManager, BookManager>();
        services.AddTransient<ICheckoutManager, CheckoutManager>();
        services.AddTransient<IDesignationManager, DesignationManager>();
        services.AddTransient<IHttpInterceptorManager, HttpInterceptorManager>();
        services.AddTransient<IPatronManager, PatronManager>();
        services.AddTransient<IRoleManager, RoleManager>();
        services.AddTransient<IUserManager, UserManager>();
        services.AddTransient(typeof(IHttpClientManager<,>), typeof(HttpClientManager<,>));
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