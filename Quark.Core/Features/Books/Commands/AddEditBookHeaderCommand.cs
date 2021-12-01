namespace Quark.Core.Features.Books.Commands;

public class AddEditBookHeaderCommand : IRequest<Result<Guid>>
{
    public Guid Id { get; set; }
    public string Barcode { get; set; }
    public string Condition { get; set; }
    public Guid BookId { get; set; }
    public AddEditBookHeaderCommand(Guid bookId)
    {
        BookId = bookId;
    }
}

internal class AddEditBookHeaderCommandHandler : IRequestHandler<AddEditBookHeaderCommand, Result<Guid>>
{
    private readonly IUnitOfWork<Guid> _unitOfWork;

    public AddEditBookHeaderCommandHandler(IUnitOfWork<Guid> unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<Result<Guid>> Handle(AddEditBookHeaderCommand request, CancellationToken cancellationToken)
    {
        var bookHeader = new BookHeader
        {
            Barcode = request.Barcode,
            Condition = request.Condition,
            BookId = request.BookId
        };
        if (request.Id == Guid.Empty)
        {
            bookHeader.Id = Guid.NewGuid();
            await _unitOfWork.Repository<BookHeader>().AddAsync(bookHeader);
        }
        else
        {
            bookHeader.Id = request.Id;
            await _unitOfWork.Repository<BookHeader>().UpdateAsync(bookHeader);
        }
        await _unitOfWork.Commit(cancellationToken);
        return await Result<Guid>.SuccessAsync(bookHeader.Id);
    }
}