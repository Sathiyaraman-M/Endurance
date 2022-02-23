namespace Quark.Core.Features.Books.Queries;

public class GetAllAuthorsQuery : IRequest<Result<List<string>>>
{

}

internal class GetAllAuthorsQueryHandler : IRequestHandler<GetAllAuthorsQuery, Result<List<string>>>
{
    private readonly IUnitOfWork<Guid> _unitOfWork;

    public GetAllAuthorsQueryHandler(IUnitOfWork<Guid> unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<Result<List<string>>> Handle(GetAllAuthorsQuery request, CancellationToken cancellationToken)
    {
        var authors = await _unitOfWork.Repository<Book>().Entities.Select(x => x.Author).Distinct().ToListAsync(cancellationToken);
        return await Result<List<string>>.SuccessAsync(authors);
    }
}