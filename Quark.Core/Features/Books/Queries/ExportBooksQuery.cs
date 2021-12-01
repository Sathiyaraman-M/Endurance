namespace Quark.Core.Features.Books.Queries;

public class ExportBooksQuery : IRequest<Result<string>>
{
    public ExportBooksQuery(string searchString)
    {
        SearchString = searchString;
    }

    public string SearchString { get; set; }
}

internal class ExportBooksQueryHandler : IRequestHandler<ExportBooksQuery, Result<string>>
{
    private readonly IUnitOfWork<Guid> _unitOfWork;
    private readonly IExcelService _excelService;

    public ExportBooksQueryHandler(IUnitOfWork<Guid> unitOfWork, IExcelService excelService)
    {
        _unitOfWork = unitOfWork;
        _excelService = excelService;
    }
    public async Task<Result<string>> Handle(ExportBooksQuery request, CancellationToken cancellationToken)
    {
        var bookSpec = new BookFilterSpecification(request.SearchString);
        var books = await _unitOfWork.Repository<Book>().Entities.Specify(bookSpec).ToListAsync(cancellationToken);
        var data = await _excelService.ExportAsync(books, mappings: new Dictionary<string, Func<Book, object>>
        {
            { "Id", x => x.Id },
            { "Book Title", x => x.Name },
            { "ISBN", x => x.ISBN },
            { "Dewey Index", x => x.DeweyIndex },
            { "Author", x => x.Author },
            { "Publisher Name", x => x.Publisher },
            { "Cost", x => x.Cost },
            { "Edition", x => x.Edition },
            { "Publication Date", x => x.PublicationYear },
            { "Description", x => x.Description },
            { "Copies", x => x.Copies },
            { "Available", x => x.AvailableCopies },
            { "Damaged", x => x.DamagedCopies },
            { "Lost", x => x.LostCopies },
            { "Unknown status", x => x.UnknownStatusCopies }
        }, sheetName: "Books", cancellationToken);
        return await Result<string>.SuccessAsync(data: data);
    }
}