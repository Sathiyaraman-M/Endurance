namespace Quark.Core.Features.Books.Commands;

public class DeleteBookHeaderCommand : IRequest<Result<Guid>>
{
    public Guid Id { get; set; }

    public DeleteBookHeaderCommand(Guid id) => Id = id;
}

internal class DeleteBookHeaderCommandHandler : IRequestHandler<DeleteBookHeaderCommand, Result<Guid>>
{
    private readonly IUnitOfWork<int> _unitOfWorkInt;
    private readonly IUnitOfWork<Guid> _unitOfWorkGuid;

    public DeleteBookHeaderCommandHandler(IUnitOfWork<Guid> unitOfWorkGuid, IUnitOfWork<int> unitOfWorkInt)
    {
        _unitOfWorkGuid = unitOfWorkGuid;
        _unitOfWorkInt = unitOfWorkInt;
    }

    public async Task<Result<Guid>> Handle(DeleteBookHeaderCommand request, CancellationToken cancellationToken)
    {
        if (await _unitOfWorkInt.Repository<Checkout>().Entities.Include(x => x.BookHeader).AnyAsync(x => x.BookHeader.Id == request.Id))
        {
            return await Result<Guid>.FailAsync("Deletion not allowed!");
        }
        var book = await _unitOfWorkGuid.Repository<BookHeader>().GetByIdAsync(request.Id);
        if (book is not null)
        {
            await _unitOfWorkGuid.Repository<BookHeader>().DeleteAsync(book);
            await _unitOfWorkGuid.Commit(cancellationToken);
            return await Result<Guid>.SuccessAsync(request.Id, "Book deleted!");
        }
        else
        {
            return await Result<Guid>.FailAsync("Book Not Found!");
        }
    }
}