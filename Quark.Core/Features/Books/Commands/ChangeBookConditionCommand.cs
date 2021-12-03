namespace Quark.Core.Features.Books.Commands;

public class ChangeBookConditionCommand : IRequest<Result<Guid>>
{
    public ChangeBookConditionCommand(string barcode, string condition)
    {
        Barcode = barcode;
        Condition = condition;
    }
    public string Barcode { get; set; }
    public string Condition { get; set; }
}

internal class ChangeBookConditionCommandHandler : IRequestHandler<ChangeBookConditionCommand, Result<Guid>>
{
    private readonly IUnitOfWork<Guid> _unitOfWork;

    public ChangeBookConditionCommandHandler(IUnitOfWork<Guid> unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<Result<Guid>> Handle(ChangeBookConditionCommand request, CancellationToken cancellationToken)
    {
        var bookHeader = await _unitOfWork.Repository<BookHeader>().Entities.FirstOrDefaultAsync(x => x.Barcode == request.Barcode, cancellationToken);
        if (bookHeader is not null)
        {
            bookHeader.Condition = request.Condition;
            await _unitOfWork.Repository<BookHeader>().UpdateAsync(bookHeader);
            await _unitOfWork.Commit(cancellationToken);
            return await Result<Guid>.SuccessAsync(bookHeader.BookId, "Book condition updated!");
        }
        return await Result<Guid>.FailAsync("Book not found!");
    }
}