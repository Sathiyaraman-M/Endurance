namespace Quark.Core.Features.Books.Commands;

public class AddEditBookCommand : IRequest<Result<int>>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ISBN { get; set; }
    public string Author { get; set; }
    public string DeweyIndex { get; set; }
    public string Publisher { get; set; }
    public string Edition { get; set; }
    public int PublicationYear { get; set; }
    public string Description { get; set; }
    public string Barcode { get; set; }
    public decimal Cost { get; set; }
    public string ImageUrl { get; set; }
    public bool IsAvailable { get; set; }
    public string Condition { get; set; }
}

internal class AddEditBookCommandHandler : IRequestHandler<AddEditBookCommand, Result<int>>
{
    private readonly IUnitOfWork<int> _unitOfWork;
    private readonly IMapper _mapper;

    public AddEditBookCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<int>> Handle(AddEditBookCommand request, CancellationToken cancellationToken)
    {
        if (await _unitOfWork.Repository<Book>().Entities.Where(p => p.Id != request.Id).AnyAsync(p => p.Barcode == request.Barcode, cancellationToken))
        {
            return await Result<int>.FailAsync("Barcode already exists.");
        }
        var book = _mapper.Map<Book>(request);
        if (string.IsNullOrWhiteSpace(request.Condition))
        {
            book.Condition = AssetStatusConstants.GoodCondition;
            book.IsAvailable = true;
        }
        if (request.Id == 0)
        {
            await _unitOfWork.Repository<Book>().AddAsync(book);
            await _unitOfWork.Commit(cancellationToken);
            return await Result<int>.SuccessAsync(book.Id, "Book Saved!");
        }
        else
        {
            if (await _unitOfWork.Repository<Book>().GetByIdAsync(request.Id) is not null)
            {
                await _unitOfWork.Repository<Book>().UpdateAsync(book);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(book.Id, "Book Updated!");
            }
            else
            {
                return await Result<int>.FailAsync("Book not found!");
            }
        }
    }
}