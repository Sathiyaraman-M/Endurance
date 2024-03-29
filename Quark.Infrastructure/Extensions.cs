﻿global using AutoMapper;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.EntityFrameworkCore;
global using Quark.Core.Domain.Common;
global using Quark.Core.Domain.Entities;
global using Quark.Core.Interfaces.Repositories;
global using Quark.Core.Interfaces.Services;
global using Quark.Core.Interfaces.Services.Identity;
global using Quark.Core.Requests.Identity;
global using Quark.Core.Responses.Identity;
global using Quark.Infrastructure.DbContexts;
global using Quark.Infrastructure.Models.Identity;
global using Quark.Infrastructure.Repositories;
global using Quark.Shared.Constants;
global using Quark.Shared.Constants.Permission;
global using Quark.Shared.Wrapper;
using Microsoft.Extensions.DependencyInjection;
using Quark.Core.Interfaces.Serialization.Serializers;
using Quark.Core.Interfaces.Services.Storage;
using Quark.Core.Interfaces.Services.Storage.Provider;
using Quark.Core.Serialization.JsonConverters;
using Quark.Core.Serialization.Options;
using Quark.Core.Serialization.Serializers;
using Quark.Infrastructure.Services.Storage;
using Quark.Infrastructure.Services.Storage.Provider;
using System.Reflection;
using System.Security.Claims;

namespace Quark.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructureMappings(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddTransient(typeof(IRepositoryAsync<,>), typeof(RepositoryAsync<,>));
        services.AddTransient<IDesignationRepository, DesignationRepository>();
        services.AddTransient<IDashboardRepository, DashboardRepository>();
        //TODO: Add Transient Services of other repositories
        services.AddTransient(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
        return services;
    }

    public static IServiceCollection AddServerStorage(this IServiceCollection services)
        => AddServerStorage(services, null);

    public static IServiceCollection AddServerStorage(this IServiceCollection services, Action<SystemTextJsonOptions> configure)
    {
        return services
            .AddScoped<IJsonSerializer, SystemTextJsonSerializer>()
            .AddScoped<IStorageProvider, ServerStorageProvider>()
            .AddScoped<IServerStorageService, ServerStorageService>()
            .AddScoped<ISyncServerStorageService, ServerStorageService>()
            .Configure<SystemTextJsonOptions>(configureOptions =>
            {
                configure?.Invoke(configureOptions);
                if (!configureOptions.JsonSerializerOptions.Converters.Any(c => c.GetType() == typeof(TimespanJsonConverter)))
                    configureOptions.JsonSerializerOptions.Converters.Add(new TimespanJsonConverter());
            });
    }
}

public static class ClaimExtensions
{
    public static void GetAllPermissions(this List<RoleClaimResponse> allPermissions)
    {
        var modules = typeof(Permissions).GetNestedTypes();

        foreach (var module in modules)
        {
            var fields = module.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

            foreach (FieldInfo fi in fields)
            {
                var propertyValue = fi.GetValue(null);

                if (propertyValue is not null)
                    allPermissions.Add(new RoleClaimResponse { Value = propertyValue.ToString(), Type = ApplicationClaimTypes.Permission, Group = module.Name });
                //TODO - take descriptions from description attribute
            }
        }
    }

    public static async Task<IdentityResult> AddPermissionClaim(this RoleManager<ApplicationRole> roleManager, ApplicationRole role, string permission)
    {
        var allClaims = await roleManager.GetClaimsAsync(role);
        if (!allClaims.Any(a => a.Type == ApplicationClaimTypes.Permission && a.Value == permission))
        {
            return await roleManager.AddClaimAsync(role, new Claim(ApplicationClaimTypes.Permission, permission));
        }

        return IdentityResult.Failed();
    }
}