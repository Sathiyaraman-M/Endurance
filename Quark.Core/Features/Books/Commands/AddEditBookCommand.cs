namespace Quark.Core.Features.Books.Commands;

public class AddEditBookCommand : IRequest<Result<Guid>>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string ISBN { get; set; }
    public string Author { get; set; }
    public string DeweyIndex { get; set; }
    public string Publisher { get; set; }
    public string Edition { get; set; }
    public int PublicationYear { get; set; } = DateTime.Today.Year;
    public string Description { get; set; }
    public decimal Cost { get; set; }
    public string ImageUrl { get; set; }
    public List<BookHeaderResponse> Headers { get; set; } = new();
    public List<Guid> DeleteBookHeaders { get; set; } = new();
}

internal class AddEditBookCommandHandler : IRequestHandler<AddEditBookCommand, Result<Guid>>
{
    private readonly IUnitOfWork<Guid> _unitOfWork;
    private readonly IMapper _mapper;

    public AddEditBookCommandHandler(IUnitOfWork<Guid> unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<Guid>> Handle(AddEditBookCommand request, CancellationToken cancellationToken)
    {
        var book = new Book
        {
            Id = request.Id,
            Name = request.Name,
            ISBN = request.ISBN,
            Author = request.Author,
            DeweyIndex = request.DeweyIndex,
            Publisher = request.Publisher,
            Edition = request.Edition,
            PublicationYear = request.PublicationYear,
            Description = request.Description,
            Cost = request.Cost,
            ImageUrl = request.ImageUrl,
            Copies = request.Headers.Count,
            AvailableCopies = request.Headers.Count(x => x.Condition == AssetStatusConstants.GoodCondition),
            LostCopies = request.Headers.Count(x => x.Condition == AssetStatusConstants.Lost),
            DamagedCopies = request.Headers.Count(x => x.Condition == AssetStatusConstants.Damaged),
            UnknownStatusCopies = request.Headers.Count(x => x.Condition == AssetStatusConstants.Unknown),
            DisposedCopies = request.Headers.Count(x => x.Condition == AssetStatusConstants.Disposed),
        };
        if (request.Id == Guid.Empty)
        {
            book.Id = Guid.NewGuid();
            await _unitOfWork.Repository<Book>().AddAsync(book);
        }
        else
        {
            if (await _unitOfWork.Repository<Book>().GetByIdAsync(request.Id) is null)
            {
                return await Result<Guid>.FailAsync("Book not found!");
            }
            await _unitOfWork.Repository<Book>().UpdateAsync(book);
        }
        foreach (var item in request.DeleteBookHeaders)
        {
            if (await _unitOfWork.Repository<Checkout>().Entities.Include(x => x.BookHeader).AnyAsync(x => x.BookHeader.Id == item))
            {
                return await Result<Guid>.FailAsync("Deletion not allowed!");
            }
            var bookHeader = await _unitOfWork.Repository<BookHeader>().GetByIdAsync(item);
            await _unitOfWork.Repository<BookHeader>().DeleteAsync(bookHeader);
        }
        foreach(var header in request.Headers)
        {
            header.BookId = book.Id;
            if (header.Id == Guid.Empty)
            {
                await _unitOfWork.Repository<BookHeader>().AddAsync(new() { Id = Guid.NewGuid(), Condition = header.Condition, Barcode = header.Barcode, BookId = header.BookId });
            }
            else
            {
                if (await _unitOfWork.Repository<BookHeader>().GetByIdAsync(header.Id) is null)
                {;
                    return await Result<Guid>.FailAsync("Book Item not found!");
                }
                await _unitOfWork.Repository<BookHeader>().UpdateAsync(new() { Id = header.Id, Condition = header.Condition, Barcode = header.Barcode, BookId = header.BookId });
            }
        }
        await _unitOfWork.Commit(cancellationToken);
        return await Result<Guid>.SuccessAsync(request.Id, request.Id == Guid.Empty 
            ? "Book added successfully!"
            : "Book updated successfully!");
    }
}