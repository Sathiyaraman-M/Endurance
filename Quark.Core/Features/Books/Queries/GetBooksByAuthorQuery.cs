namespace Quark.Core.Features.Books.Queries;

public class GetBooksByAuthorQuery : IRequest<Result<AuthorResponse>>
{
    public GetBooksByAuthorQuery(string author) => Author = author;

    public string Author { get; }
}

internal class GetBooksByAuthorQueryHandler : IRequestHandler<GetBooksByAuthorQuery, Result<AuthorResponse>>
{
    private readonly IUnitOfWork<Guid> _unitOfWork;

    public GetBooksByAuthorQueryHandler(IUnitOfWork<Guid> unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<Result<AuthorResponse>> Handle(GetBooksByAuthorQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<Book, KeyValuePair<Guid, string>>> expression = e => new KeyValuePair<Guid, string>(e.Id, e.Name);
        if(!await _unitOfWork.Repository<Book>().Entities.AnyAsync(x => x.Author == request.Author, cancellationToken))
        {
            return await Result<AuthorResponse>.FailAsync($"No books found with author \'{request.Author}\'");
        }
        var books = await _unitOfWork.Repository<Book>().Entities.Where(x => x.Author == request.Author).Select(expression).ToDictionaryAsync(x => x.Key, x => x.Value,cancellationToken);
        var response = new AuthorResponse { Author = request.Author, Books = books };
        return await Result<AuthorResponse>.SuccessAsync(response);
    }
}