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
    private readonly IUnitOfWork<int> _unitOfWork;
    private readonly IExcelService _excelService;

    public ExportBooksQueryHandler(IUnitOfWork<int> unitOfWork, IExcelService excelService)
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
            { "Barcode", x => x.Barcode },
            { "Author", x => x.Author },
            { "Publisher Name", x => x.Publisher },
            { "Cost", x => x.Cost },
            { "Book latest condition", x => x.Condition },
            { "Edition", x => x.Edition },
            { "Publication Date", x => x.PublicationYear },
            { "Description", x => x.Description }
        }, sheetName: "Books", cancellationToken);
        return await Result<string>.SuccessAsync(data: data);
    }
}