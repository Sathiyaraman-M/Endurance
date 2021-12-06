using System.Text;

namespace Quark.Core.Features.Patrons.Queries;

public class ExportPatronsQuery : IRequest<Result<string>>
{
    public ExportPatronsQuery(string searchString = "")
    {
        SearchString = searchString;
    }

    public string SearchString { get; set; }
}

internal class ExportPatronsQueryHandler : IRequestHandler<ExportPatronsQuery, Result<string>>
{
    private readonly IUnitOfWork<Guid> _unitOfWork;
    private readonly IExcelService _excelService;

    public ExportPatronsQueryHandler(IUnitOfWork<Guid> unitOfWork, IExcelService excelService)
    {
        _unitOfWork = unitOfWork;
        _excelService = excelService;
    }

    public async Task<Result<string>> Handle(ExportPatronsQuery request, CancellationToken cancellationToken)
    {
        var bookSpec = new PatronFilterSpecification(request.SearchString);
        var books = await _unitOfWork.Repository<Patron>().Entities.Specify(bookSpec).Include(x => x.Checkouts).ToListAsync(cancellationToken);
        var data = await _excelService.ExportAsync(books, mappings: new Dictionary<string, Func<Patron, object>>
        {
            { "Id", x => x.Id },
            { "RegisterId", x => x.RegisterId},
            { "Patron Name", x => new StringBuilder(x.FirstName).Append(' ').Append(x.LastName).ToString() },
            { "Email", x => x.Email },
            { "Mobile", x => x.Mobile },
            { "Current Fees", x => x.CurrentFees },
            { "Date of birth", x => x.DateOfBirth.ToString("dd/MM/yyyy") },
            { "Address", x => x.Address },
            { "Total checkouts", x => x.Checkouts.Count() },
            { "Remaining check in", x => x.Checkouts.Count(s => s.CheckedOutUntil.HasValue) },
            { "Simultaneous Multiple checkout limit", x => x.MultipleCheckoutLimit }
        }, sheetName: "Patrons", cancellationToken);
        return await Result<string>.SuccessAsync(data: data);
    }
}