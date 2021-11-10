using MediatR;
using Microsoft.EntityFrameworkCore;
using Quark.Core.Domain.Entities;
using Quark.Core.Interfaces.Repositories;
using Quark.Shared.Wrapper;

namespace Quark.Core.Features.Books.Commands;

public class DeleteBookCommand : IRequest<Result<int>>
{
    public int Id { get; set; }
}

internal class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, Result<int>>
{
    private readonly IUnitOfWork<int> _unitOfWork;

    public DeleteBookCommandHandler(IUnitOfWork<int> unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<Result<int>> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
    {
        if (await _unitOfWork.Repository<Checkout>().Entities.Include(x => x.Book).AnyAsync(x => x.Book.Id == request.Id))
        {
            return await Result<int>.FailAsync("Deletion not allowed!");
        }
        var book = await _unitOfWork.Repository<Book>().GetByIdAsync(request.Id);
        if (book is not null)
        {
            await _unitOfWork.Repository<Book>().DeleteAsync(book);
            await _unitOfWork.Commit(cancellationToken);
            return await Result<int>.SuccessAsync(request.Id, "Book deleted!");
        }
        else
        {
            return await Result<int>.FailAsync("Book Not Found!");
        }
    }
}