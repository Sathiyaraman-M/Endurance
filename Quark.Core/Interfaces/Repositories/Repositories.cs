namespace Quark.Core.Interfaces.Repositories;

public interface IDesignationRepository
{
    Task<bool> IsDesignationUsed(int Id);
}
public interface IDashboardRepository
{
    Task<int> GetUsersCount();
}