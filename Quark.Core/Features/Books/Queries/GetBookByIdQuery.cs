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
        var book = await _unitOfWork.Repository<Book>().Entities.Include(x => x.BookHeaders).FirstAsync(x => x.Id == request.Id);
        var bookResponse = new BookResponse
        {
            Id = book.Id,
            Name = book.Name,
            ISBN = book.ISBN,
            Cost = book.Cost,
            PublicationYear = book.PublicationYear,
            Publisher = book.Publisher,
            Edition = book.Edition,
            DeweyIndex = book.DeweyIndex,
            Author = book.Author,
            Copies = book.Copies,
            AvailableCopies = book.AvailableCopies,
            DamagedCopies = book.DamagedCopies,
            Description = book.Description,
            DisposedCopies = book.DisposedCopies,
            LostCopies = book.LostCopies,
            ImageUrl = book.ImageUrl,
            UnknownStatusCopies = book.UnknownStatusCopies,
            BookHeaders = _mapper.Map<List<BookHeaderResponse>>(book.BookHeaders),
        };
        return await Result<BookResponse>.SuccessAsync(bookResponse);
    }
}