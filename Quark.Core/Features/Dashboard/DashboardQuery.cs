using MediatR;
using Quark.Core.Domain.Entities;
using Quark.Core.Interfaces.Repositories;
using Quark.Core.Responses;
using Quark.Shared.Wrapper;

namespace Quark.Core.Features.Dashboard;

public class DashboardQuery : IRequest<Result<DashboardResponse>>
{

}

internal class DashboardQueryHandler : IRequestHandler<DashboardQuery, Result<DashboardResponse>>
{
    private readonly IUnitOfWork<int> _unitOfWork;
    private readonly IDashboardRepository _repository;

    public DashboardQueryHandler(IUnitOfWork<int> unitOfWork, IDashboardRepository repository)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
    }

    public async Task<Result<DashboardResponse>> Handle(DashboardQuery request, CancellationToken cancellationToken)
    {
        var model = new DashboardResponse
        {
            UsersCount = await _repository.GetUsersCount(),
            PatronsCount = await _unitOfWork.Repository<Patron>().CountAsync(),
            CheckoutsCount = await _unitOfWork.Repository<Checkout>().CountAsync(),
            BooksCount = await _unitOfWork.Repository<Book>().CountAsync()
        };
        return await Result<DashboardResponse>.SuccessAsync(model);
    }
}