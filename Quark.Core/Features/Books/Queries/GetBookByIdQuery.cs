namespace Quark.Core.Features.Books.Queries;

public class GetBookByIdQuery : IRequest<Result<BookResponse>>
{
    public Guid Id { get; set; }

    public GetBookByIdQuery(Guid id) => Id = id;
}

internal class GetBookBydIdQueryHandler : IRequestHandler<GetBookByIdQuery, Result<BookResponse>>
{
    private readonly IUnitOfWork<Guid> _unitOfWork;
    private readonly IMapper _mapper;

    public GetBookBydIdQueryHandler(IUnitOfWork<Guid> unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<BookResponse>> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
    {
        var book = _mapper.Map<BookResponse>(await _unitOfWork.Repository<Book>().Entities.Include(x => x.BookHeaders).FirstAsync(x => x.Id == request.Id));
        return await Result<BookResponse>.SuccessAsync(book);
    }
}