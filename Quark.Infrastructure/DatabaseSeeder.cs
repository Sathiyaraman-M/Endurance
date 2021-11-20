using Microsoft.Extensions.Logging;
using Quark.Infrastructure.Extensions;

namespace Quark.Infrastructure;

public class DatabaseSeeder : IDatabaseSeeder
{
    private readonly LibraryDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly ILogger<DatabaseSeeder> _logger;

    public DatabaseSeeder(LibraryDbContext dbContext, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, ILogger<DatabaseSeeder> logger)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
    }

    public void Initialize()
    {
        SeedAdministratorUser();
        SeedBasicUser();
        //SeedAssetAvailabilityStatuses();
        _dbContext.SaveChanges();
    }

    //private void SeedAssetAvailabilityStatuses()
    //{
    //    Task.Run(async () =>
    //    {
    //        if (!(await _dbContext.AssetAvailabilityStatuses.AnyAsync()))
    //        {
    //            await _dbContext.Set<AssetAvailability>().AddAsync(new() { Id = 1, Name = AssetStatusConstants.GoodCondition, Description = "The item is in good condition." });
    //            await _dbContext.Set<AssetAvailability>().AddAsync(new() { Id = 2, Name = AssetStatusConstants.Lost, Description = "The item is lost." });
    //            await _dbContext.Set<AssetAvailability>().AddAsync(new() { Id = 3, Name = AssetStatusConstants.Destroyed, Description = "The item has been destroyed." });
    //            await _dbContext.Set<AssetAvailability>().AddAsync(new() { Id = 4, Name = AssetStatusConstants.Unknown, Description = "The item is in unknown whereabouts and condition." });
    //        }
    //    });
    //}

    private void SeedAdministratorUser()
    {
        Task.Run(async () =>
        {
            var adminRole = new ApplicationRole(RoleConstants.AdministratorRole, "Administrator role with full permissions");
            var adminRoleInDb = await _roleManager.FindByNameAsync(RoleConstants.AdministratorRole);
            if (adminRoleInDb == null)
            {
                await _roleManager.CreateAsync(adminRole);
                adminRoleInDb = await _roleManager.FindByNameAsync(RoleConstants.AdministratorRole);
                _logger.LogInformation("Seeded Administrator Role.");
            }
                //Check if User Exists
                var superUser = new ApplicationUser
            {
                FullName = "Administrator",
                Email = "bruce@wayne-enterprises.com",
                UserName = "Admin",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                CreatedOn = DateTime.Now,
                IsActive = true
            };
            var superUserInDb = await _userManager.FindByEmailAsync(superUser.Email);
            if (superUserInDb == null)
            {
                await _userManager.CreateAsync(superUser, RoleConstants.DefaultPassword);
                var result = await _userManager.AddToRoleAsync(superUser, RoleConstants.AdministratorRole);
                if (result.Succeeded)
                {
                    _logger.LogInformation("Seeded Default SuperAdmin User.");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        _logger.LogError(error.Description);
                    }
                }
            }
            foreach (var permission in Permissions.GetRegisteredPermissions())
            {
                await _roleManager.AddPermissionClaim(adminRoleInDb, permission);
            }
        }).GetAwaiter().GetResult();
    }

    private void SeedBasicUser()
    {
        Task.Run(async () =>
        {
                //Check if Role Exists
                var basicRole = new ApplicationRole(RoleConstants.BasicRole, "Basic role with default permissions");
            var basicRoleInDb = await _roleManager.FindByNameAsync(RoleConstants.BasicRole);
            if (basicRoleInDb == null)
            {
                await _roleManager.CreateAsync(basicRole);
                _logger.LogInformation("Seeded Basic Role.");
            }
                //Check if User Exists
                var basicUser = new ApplicationUser
            {
                FullName = "Lucius Fox",
                Email = "fox@wayne-enterprises.com",
                UserName = "Fox",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                CreatedOn = DateTime.Now,
                IsActive = true
            };
            var basicUserInDb = await _userManager.FindByEmailAsync(basicUser.Email);
            if (basicUserInDb == null)
            {
                await _userManager.CreateAsync(basicUser, RoleConstants.DefaultPassword);
                await _userManager.AddToRoleAsync(basicUser, RoleConstants.BasicRole);
                _logger.LogInformation("Seeded User with Basic Role.");
            }
        }).GetAwaiter().GetResult();
    }
}