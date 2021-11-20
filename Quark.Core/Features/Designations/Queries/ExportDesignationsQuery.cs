namespace Quark.Core.Features.Designations.Queries;

public class ExportDesignationsQuery : IRequest<Result<string>>
{
    public ExportDesignationsQuery(string searchString = "")
    {
        SearchString = searchString;
    }

    public string SearchString { get; set; }
}

internal class ExportDesignationsQueryHandler : IRequestHandler<ExportDesignationsQuery, Result<string>>
{
    private readonly IUnitOfWork<int> _unitOfWork;
    private readonly IExcelService _excelService;

    public ExportDesignationsQueryHandler(IUnitOfWork<int> unitOfWork, IExcelService excelService)
    {
        _unitOfWork = unitOfWork;
        _excelService = excelService;
    }
    public async Task<Result<string>> Handle(ExportDesignationsQuery request, CancellationToken cancellationToken)
    {
        var desigSpec = new DesignationFilterSpecification(request.SearchString);
        var designations = await _unitOfWork.Repository<Designation>().Entities.Specify(desigSpec).ToListAsync(cancellationToken);
        var data = await _excelService.ExportAsync(designations, mappings: new Dictionary<string, Func<Designation, object>>
        {
            { "Id", x => x.Id },
            { "Name", x=> x.Name }
        }, sheetName: "Designations", cancellationToken);
        return await Result<string>.SuccessAsync(data: data);
    }
}