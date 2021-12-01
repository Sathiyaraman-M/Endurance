namespace Quark.Core.Features.Dashboard;

public class DashboardQuery : IRequest<Result<DashboardResponse>>
{

}

internal class DashboardQueryHandler : IRequestHandler<DashboardQuery, Result<DashboardResponse>>
{
    private readonly IUnitOfWork<int> _unitOfWorkInt;
    private readonly IUnitOfWork<Guid> _unitOfWorkGuid;

    public DashboardQueryHandler(IUnitOfWork<int> unitOfWorkInt, IUnitOfWork<Guid> unitOfWorkGuid)
    {
        _unitOfWorkInt = unitOfWorkInt;
        _unitOfWorkGuid = unitOfWorkGuid;
    }

    public async Task<Result<DashboardResponse>> Handle(DashboardQuery request, CancellationToken cancellationToken)
    {
        var model = new DashboardResponse
        {
            PatronsCount = await _unitOfWorkInt.Repository<Patron>().CountAsync(),
            CheckoutsCount = await _unitOfWorkInt.Repository<Checkout>().CountAsync(),
            BooksCount = await _unitOfWorkGuid.Repository<Book>().CountAsync(),
            CheckInTodayCount = await _unitOfWorkInt.Repository<Checkout>().Entities.CountAsync(x => x.CheckedOutUntil.Value.Date == DateTime.Today.Date),
            CheckInPending = await _unitOfWorkInt.Repository<Checkout>().Entities.CountAsync(x => !x.CheckedOutUntil.HasValue),
        };
        return await Result<DashboardResponse>.SuccessAsync(model);
    }
}