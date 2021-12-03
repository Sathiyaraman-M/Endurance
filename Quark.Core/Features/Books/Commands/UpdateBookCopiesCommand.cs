namespace Quark.Core.Features.Books.Commands;

public class UpdateBookCopiesCommand : IRequest<Result<Guid>>
{
    public Guid Id { get; set; }

    public UpdateBookCopiesCommand(Guid id)
    {
        Id = id;
    }
}

internal class UpdateBookCopiesCommandHandler : IRequestHandler<UpdateBookCopiesCommand, Result<Guid>>
{
    private readonly IUnitOfWork<Guid> _unitOfWork;

    public UpdateBookCopiesCommandHandler(IUnitOfWork<Guid> unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<Result<Guid>> Handle(UpdateBookCopiesCommand request, CancellationToken cancellationToken)
    {
        var book = await _unitOfWork.Repository<Book>().GetByIdAsync(request.Id);
        var bookHeaders = await _unitOfWork.Repository<BookHeader>().Entities.Where(x => x.BookId == book.Id).ToListAsync(cancellationToken);
        book.Copies = bookHeaders.Count;
        book.AvailableCopies = bookHeaders.Count(x => x.Condition == AssetStatusConstants.GoodCondition);
        book.LostCopies = bookHeaders.Count(x => x.Condition == AssetStatusConstants.Lost);
        book.DamagedCopies = bookHeaders.Count(x => x.Condition == AssetStatusConstants.Damaged);
        book.UnknownStatusCopies = bookHeaders.Count(x => x.Condition == AssetStatusConstants.Unknown);
        book.DisposedCopies = bookHeaders.Count(x => x.Condition == AssetStatusConstants.Disposed);
        await _unitOfWork.Repository<Book>().UpdateAsync(book);
        await _unitOfWork.Commit(cancellationToken);
        return await Result<Guid>.SuccessAsync(book.Id, "Updated book copies");
    }
}