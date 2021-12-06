namespace Quark.Core.Features.Dashboard;

public class DashboardQuery : IRequest<Result<DashboardResponse>>
{

}

internal class DashboardQueryHandler : IRequestHandler<DashboardQuery, Result<DashboardResponse>>
{
    private readonly IUnitOfWork<Guid> _unitOfWork;

    public DashboardQueryHandler(IUnitOfWork<Guid> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<DashboardResponse>> Handle(DashboardQuery request, CancellationToken cancellationToken)
    {
        var model = new DashboardResponse
        {
            PatronsCount = await _unitOfWork.Repository<Patron>().CountAsync(),
            CheckoutsCount = await _unitOfWork.Repository<Checkout>().CountAsync(),
            BooksCount = await _unitOfWork.Repository<Book>().CountAsync(),
            CheckInTodayCount = await _unitOfWork.Repository<Checkout>().Entities.CountAsync(x => x.CheckedOutUntil.Value.Date == DateTime.Today.Date, cancellationToken),
            CheckInPending = await _unitOfWork.Repository<Checkout>().Entities.CountAsync(x => !x.CheckedOutUntil.HasValue, cancellationToken),
        };
        return await Result<DashboardResponse>.SuccessAsync(model);
    }
}