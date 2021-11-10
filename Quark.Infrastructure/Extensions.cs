using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Quark.Core.Interfaces.Repositories;
using Quark.Core.Responses.Identity;
using Quark.Infrastructure.Models.Identity;
using Quark.Infrastructure.Repositories;
using Quark.Shared.Constants.Permission;
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