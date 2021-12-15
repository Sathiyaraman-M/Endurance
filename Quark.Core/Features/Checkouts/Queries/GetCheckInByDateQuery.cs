namespace Quark.Core.Features.Checkouts.Queries;

public class GetCheckInByDateQuery : IRequest<Result<List<CheckoutResponse>>>
{
    public GetCheckInByDateQuery(DateTime date) => Date = date;

    public DateTime Date { get; set; }
}

internal class GetCheckInByDateQueryHandler : IRequestHandler<GetCheckInByDateQuery, Result<List<CheckoutResponse>>>
{
    private readonly IUnitOfWork<Guid> _unitOfWork;

    public GetCheckInByDateQueryHandler(IUnitOfWork<Guid> unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<Result<List<CheckoutResponse>>> Handle(GetCheckInByDateQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<Checkout, CheckoutResponse>> expression = e => new CheckoutResponse
        {
            Id = e.Id,
            BookId = e.BookHeaderId,
            BookName = e.BookHeader.Book.Name,
            DeweyIndex = e.BookHeader.Book.DeweyIndex,
            BookBarcode = e.BookHeader.Barcode,
            PatronId = e.PatronId,
            PatronRegisterId = e.Patron.RegisterId,
            PatronName = e.Patron.FirstName + " " + e.Patron.LastName,
            CheckedOutSince = e.CheckedOutSince,
            ExpectedCheckInDate = e.ExpectedCheckInDate,
            CheckedOutUntil = e.CheckedOutUntil
        };
        var checkInToday = await _unitOfWork.Repository<Checkout>().Entities
                                .Where(x => x.CheckedOutUntil.HasValue && x.CheckedOutUntil.Value == request.Date)
                                .Include(x => x.BookHeader).Include(x => x.Patron)
                                .Select(expression)
                                .ToListAsync(cancellationToken);
        return await Result<List<CheckoutResponse>>.SuccessAsync(checkInToday);
    }
}