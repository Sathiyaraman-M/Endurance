using MediatR;
using Microsoft.EntityFrameworkCore;
using Quark.Core.Domain.Entities;
using Quark.Core.Extensions;
using Quark.Core.Interfaces.Repositories;
using Quark.Core.Responses;
using Quark.Core.Specifications;
using Quark.Shared.Wrapper;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace Quark.Core.Features.Patrons.Queries;

public class GetAllPatronsQuery : IRequest<PaginatedResult<PatronResponse>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string SearchString { get; set; }
    public string[] OrderBy { get; set; }

    public GetAllPatronsQuery(int pageNumber, int pageSize, string searchString, string orderBy)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        SearchString = searchString;
        if (!string.IsNullOrWhiteSpace(orderBy))
        {
            OrderBy = orderBy.Split(',');
        }
    }
}

public class GetAllPatronsQueryHandler : IRequestHandler<GetAllPatronsQuery, PaginatedResult<PatronResponse>>
{
    private readonly IUnitOfWork<int> _unitOfWork;

    public GetAllPatronsQueryHandler(IUnitOfWork<int> unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<PaginatedResult<PatronResponse>> Handle(GetAllPatronsQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<Patron, PatronResponse>> expression = e => new PatronResponse
        {
            Id = e.Id,
            FirstName = e.FirstName,
            LastName = e.LastName,
            Address = e.Address,
            DateOfBirth = e.DateOfBirth,
            Email = e.Email,
            Mobile = e.Mobile,
            RegisterId = e.RegisterId,
            CurrentFees = e.CurrentFees,
            Issued = e.Issued,
            CheckoutsCount = e.Checkouts.Count(),
            CheckoutsPending = e.Checkouts.Where(x => x.CheckedOutUntil.HasValue).Count()

        };
        var patronSpec = new PatronFilterSpecification(request.SearchString);
        if (request.OrderBy?.Any() != true)
        {
            return await _unitOfWork.Repository<Patron>().Entities.Include(x => x.Checkouts).Specify(patronSpec)
                .Select(expression).ToPaginatedListAsync(request.PageNumber, request.PageSize);

        }
        else
        {
            return await _unitOfWork.Repository<Patron>().Entities.Include(x => x.Checkouts).Specify(patronSpec)
                .OrderBy(string.Join(",", request.OrderBy))
                .Select(expression).ToPaginatedListAsync(request.PageNumber, request.PageSize);
        }
    }
}