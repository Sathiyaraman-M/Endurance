using MediatR;
using Quark.Core.Domain.Entities;
using Quark.Core.Interfaces.Repositories;
using Quark.Shared.Wrapper;

namespace Quark.Core.Features.Books.Commands;

public class ChangeBookConditionCommand : IRequest<Result<string>>
{
    public ChangeBookConditionCommand(int id, string condition)
    {
        Id = id;
        Condition = condition;
    }
    public int Id { get; set; }
    public string Condition { get; set; }
}

internal class ChangeBookConditionCommandHandler : IRequestHandler<ChangeBookConditionCommand, Result<string>>
{
    private readonly IUnitOfWork<int> _unitOfWork;

    public ChangeBookConditionCommandHandler(IUnitOfWork<int> unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<Result<string>> Handle(ChangeBookConditionCommand request, CancellationToken cancellationToken)
    {
        var book = await _unitOfWork.Repository<Book>().GetByIdAsync(request.Id);
        if (book is not null)
        {
            book.Condition = request.Condition;
            await _unitOfWork.Repository<Book>().UpdateAsync(book);
            await _unitOfWork.Commit(cancellationToken);
            return await Result<string>.SuccessAsync(book.Condition, "Book condition updated!");
        }
        return await Result<string>.FailAsync("Book not found!");
    }
}