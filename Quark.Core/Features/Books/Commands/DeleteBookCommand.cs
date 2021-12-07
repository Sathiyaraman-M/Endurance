namespace Quark.Core.Features.Books.Commands;

public class DeleteBookCommand : IRequest<Result<Guid>>
{
    public Guid Id { get; set; }

    public DeleteBookCommand(Guid id) => Id = id;
}

internal class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, Result<Guid>>
{
    private readonly IUnitOfWork<Guid> _unitOfWork;

    public DeleteBookCommandHandler(IUnitOfWork<Guid> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
    {
        var book = await _unitOfWork.Repository<Book>().Entities.Include(x => x.BookHeaders).FirstAsync(x => x.Id == request.Id, cancellationToken);
        if (book is not null)
        {
            if (await _unitOfWork.Repository<Checkout>().Entities.Include(x => x.BookHeader).AnyAsync(x => book.BookHeaders.Any(y => y.Id == x.BookHeader.Id), cancellationToken))
            {
                return await Result<Guid>.FailAsync("Deletion not allowed!");
            }
            await _unitOfWork.Repository<Book>().DeleteAsync(book);
            await _unitOfWork.Commit(cancellationToken);
            return await Result<Guid>.SuccessAsync(request.Id, "Book deleted!");
        }
        else
        {
            return await Result<Guid>.FailAsync("Book Not Found!");
        }
    }
}