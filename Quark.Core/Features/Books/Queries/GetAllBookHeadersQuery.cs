namespace Quark.Core.Features.Books.Queries;

public class GetAllBookHeadersQuery : IRequest<Result<List<BookHeaderResponse>>>
{
    public Guid Id { get; set; }

    public GetAllBookHeadersQuery(Guid id)
    {
        Id = id;
    }
}

internal class GetAllBookHeadersQueryHandler : IRequestHandler<GetAllBookHeadersQuery, Result<List<BookHeaderResponse>>>
{
    private readonly IUnitOfWork<Guid> _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllBookHeadersQueryHandler(IUnitOfWork<Guid> unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<List<BookHeaderResponse>>> Handle(GetAllBookHeadersQuery request, CancellationToken cancellationToken)
    {
        var bookHeaders = await _unitOfWork.Repository<BookHeader>().Entities.Where(x => x.BookId == request.Id).ToListAsync(cancellationToken);
        var bookHeaderResponses = _mapper.Map<List<BookHeaderResponse>>(bookHeaders);
        return await Result<List<BookHeaderResponse>>.SuccessAsync(bookHeaderResponses);
    }
}