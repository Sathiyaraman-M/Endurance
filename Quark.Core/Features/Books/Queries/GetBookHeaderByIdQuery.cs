namespace Quark.Core.Features.Books.Queries;

public class GetBookHeaderByIdQuery : IRequest<Result<BookHeaderResponse>>
{
    public Guid Id { get; set; }

    public GetBookHeaderByIdQuery(Guid id) => Id = id;
}

internal class GetBookHeaderByIdQueryHandler : IRequestHandler<GetBookHeaderByIdQuery, Result<BookHeaderResponse>>
{
    private readonly IUnitOfWork<Guid> _unitOfWork;

    public GetBookHeaderByIdQueryHandler(IUnitOfWork<Guid> unitOfWork) => _unitOfWork = unitOfWork;
    public async Task<Result<BookHeaderResponse>> Handle(GetBookHeaderByIdQuery request, CancellationToken cancellationToken)
    {
        var bookHeader = await _unitOfWork.Repository<BookHeader>().GetByIdAsync(request.Id);
        var bookHeaderResponse = new BookHeaderResponse()
        {
            Id = bookHeader.Id,
            Barcode = bookHeader.Barcode,
            Condition = bookHeader.Condition,
            BookId = bookHeader.BookId
        };
        return await Result<BookHeaderResponse>.SuccessAsync(bookHeaderResponse);
    }
}