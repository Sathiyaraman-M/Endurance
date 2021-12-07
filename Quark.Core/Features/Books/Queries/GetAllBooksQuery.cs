using System.Linq.Dynamic.Core;

namespace Quark.Core.Features.Books.Queries;

public class GetAllBooksQuery : IRequest<PaginatedResult<BookResponse>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string SearchString { get; set; }
    public string[] OrderBy { get; set; }

    public GetAllBooksQuery(int pageNumber, int pageSize, string searchString, string orderBy)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        SearchString = searchString;
        if (!string.IsNullOrWhiteSpace(orderBy))
        {
            OrderBy = orderBy.Split(',');
        }
    }
}

internal class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQuery, PaginatedResult<BookResponse>>
{
    private readonly IUnitOfWork<Guid> _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllBooksQueryHandler(IUnitOfWork<Guid> unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public Task<PaginatedResult<BookResponse>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<Book, BookResponse>> expression = e => new BookResponse
        {
            Id = e.Id,
            Name = e.Name,
            Author = e.Author,
            ISBN = e.ISBN,
            DeweyIndex = e.DeweyIndex,
            Description = e.Description,
            Publisher = e.Publisher,
            PublicationYear = e.PublicationYear,
            Edition = e.Edition,
            Cost = e.Cost,
            ImageUrl = e.ImageUrl,
            Copies = e.Copies,
            AvailableCopies = e.AvailableCopies,
            DamagedCopies = e.DamagedCopies,
            LostCopies = e.LostCopies,
            UnknownStatusCopies = e.UnknownStatusCopies,
            BookHeaders = _mapper.Map<List<BookHeaderResponse>>(e.BookHeaders)
        };
        var bookSpec = new BookFilterSpecification(request.SearchString);
        if (request.OrderBy?.Any() != true)
        {
            return _unitOfWork.Repository<Book>().Entities
                .Specify(bookSpec).Select(expression)
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);
        }
        else
        {
            return _unitOfWork.Repository<Book>().Entities
                .Specify(bookSpec).Select(expression).OrderBy(string.Join(",", request.OrderBy))
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);
        }
    }
}