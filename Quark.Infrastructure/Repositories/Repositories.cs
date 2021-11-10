using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Quark.Core.Interfaces.Repositories;
using Quark.Infrastructure.Models.Identity;

namespace Quark.Infrastructure.Repositories;
public class DesignationRepository : IDesignationRepository
{
    private readonly UserManager<ApplicationUser> _userManager;

    public DesignationRepository(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<bool> IsDesignationUsed(int Id)
    {
        return await _userManager.Users.AnyAsync(x => x.DesignationId == Id);
    }
}
public class DashboardRepository : IDashboardRepository
{
    private readonly UserManager<ApplicationUser> _userManager;

    public DashboardRepository(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<int> GetUsersCount() => await _userManager.Users.CountAsync();
}