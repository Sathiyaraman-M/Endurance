namespace Quark.Core.Features.Books.Commands;

public class ChangeBookConditionCommand : IRequest<Result<string>>
{
    public ChangeBookConditionCommand(string barcode, string condition)
    {
        Barcode = barcode;
        Condition = condition;
    }
    public string Barcode { get; set; }
    public string Condition { get; set; }
}

internal class ChangeBookConditionCommandHandler : IRequestHandler<ChangeBookConditionCommand, Result<string>>
{
    private readonly IUnitOfWork<Guid> _unitOfWork;

    public ChangeBookConditionCommandHandler(IUnitOfWork<Guid> unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<Result<string>> Handle(ChangeBookConditionCommand request, CancellationToken cancellationToken)
    {
        var book = await _unitOfWork.Repository<BookHeader>().Entities.FirstOrDefaultAsync(x => x.Barcode == request.Barcode, cancellationToken);
        if (book is not null)
        {
            book.Condition = request.Condition;
            await _unitOfWork.Repository<BookHeader>().UpdateAsync(book);
            await _unitOfWork.Commit(cancellationToken);
            return await Result<string>.SuccessAsync(book.Condition, "Book condition updated!");
        }
        return await Result<string>.FailAsync("Book not found!");
    }
}