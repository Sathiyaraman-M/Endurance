using System.Linq.Dynamic.Core;

namespace Quark.Core.Features.Checkouts.Queries;

public class GetAllCheckoutsQuery : IRequest<PaginatedResult<CheckoutResponse>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string SearchString { get; set; }
    public string[] OrderBy { get; set; }

    public GetAllCheckoutsQuery(int pageNumber, int pageSize, string searchString, string orderBy)
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

internal class GetAllCheckoutQueryHandler : IRequestHandler<GetAllCheckoutsQuery, PaginatedResult<CheckoutResponse>>
{
    private readonly IUnitOfWork<int> _unitOfWork;

    public GetAllCheckoutQueryHandler(IUnitOfWork<int> unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<PaginatedResult<CheckoutResponse>> Handle(GetAllCheckoutsQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<Checkout, CheckoutResponse>> expression = e => new CheckoutResponse
        {
            Id = e.Id,
            BookId = e.BookId,
            BookName = e.Book.Name,
            DeweyIndex = e.Book.DeweyIndex,
            BookBarcode = e.Book.Barcode,
            PatronId = e.PatronId,
            PatronRegisterId = e.Patron.RegisterId,
            PatronName = e.Patron.FirstName + " " + e.Patron.LastName,
            CheckedOutSince = e.CheckedOutSince,
            ExpectedCheckInDate = e.ExpectedCheckInDate,
            CheckedOutUntil = e.CheckedOutUntil
        };
        var checkoutSpec = new CheckoutFilterSpecification(request.SearchString);
        if (request.OrderBy?.Any() != true)
        {
            var list = await _unitOfWork.Repository<Checkout>().Entities
                .Include(x => x.Book).Include(x => x.Patron)
                .Specify(checkoutSpec).Select(expression)
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);
            return list;
        }
        else
        {
            var list = await _unitOfWork.Repository<Checkout>().Entities
                .Include(x => x.Book).Include(x => x.Patron)
                .Specify(checkoutSpec).Select(expression).OrderBy(string.Join(",", request.OrderBy))
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);
            return list;
        }
    }
}