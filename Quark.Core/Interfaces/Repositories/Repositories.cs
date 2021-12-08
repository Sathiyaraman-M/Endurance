namespace Quark.Core.Interfaces.Repositories;

public interface IDesignationRepository
{
    bool IsDesignationUsed(Guid Id);
}
public interface IDashboardRepository
{
    Task<int> GetUsersCount();
}