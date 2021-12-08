namespace Quark.Infrastructure.Repositories;
public class DesignationRepository : IDesignationRepository
{
    private readonly UserManager<ApplicationUser> _userManager;

    public DesignationRepository(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public bool IsDesignationUsed(Guid Id)
    {
        return false; //return await _userManager.Users.AnyAsync(x => x.DesignationId == Id);
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