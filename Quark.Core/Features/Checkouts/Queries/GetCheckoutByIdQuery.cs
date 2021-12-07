namespace Quark.Core.Features.Checkouts.Queries;

public class GetCheckoutByIdQuery : IRequest<Result<CheckoutResponse>>
{
    public Guid Id { get; set; }
    public GetCheckoutByIdQuery(Guid id) => Id = id;
}

internal class GetCheckoutBydIdQueryHandler : IRequestHandler<GetCheckoutByIdQuery, Result<CheckoutResponse>>
{
    private readonly IUnitOfWork<Guid> _unitOfWork;
    public GetCheckoutBydIdQueryHandler(IUnitOfWork<Guid> unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<Result<CheckoutResponse>> Handle(GetCheckoutByIdQuery request, CancellationToken cancellationToken)
    {
        var checkout = await _unitOfWork.Repository<Checkout>().Entities.Include(x => x.BookHeader).ThenInclude(x => x.Book).Include(x => x.Patron).FirstAsync(x => x.Id == request.Id);
        var checkoutResponse = new CheckoutResponse
        {
            Id = checkout.Id,
            BookId = checkout.BookHeaderId,
            BookName = checkout.BookHeader.Book.Name,
            DeweyIndex = checkout.BookHeader.Book.DeweyIndex,
            BookBarcode = checkout.BookHeader.Barcode,
            PatronId = checkout.PatronId,
            PatronRegisterId = checkout.Patron.RegisterId,
            PatronName = checkout.Patron.FirstName + " " + checkout.Patron.LastName,
            CheckedOutSince = checkout.CheckedOutSince,
            ExpectedCheckInDate = checkout.ExpectedCheckInDate,
            CheckedOutUntil = checkout.CheckedOutUntil
        };
        return await Result<CheckoutResponse>.SuccessAsync(checkoutResponse);
    }
}