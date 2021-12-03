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
        var book = _mapper.Map<Book>(request);
        if (request.Id == Guid.Empty)
        {
            book.Id = Guid.NewGuid();
            await _unitOfWork.Repository<Book>().AddAsync(book);
            await _unitOfWork.Commit(cancellationToken);
            return await Result<Guid>.SuccessAsync(book.Id, "Book Saved!");
        }
        else
        {
            if (await _unitOfWork.Repository<Book>().GetByIdAsync(request.Id) is not null)
            {
                await _unitOfWork.Repository<Book>().UpdateAsync(book);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<Guid>.SuccessAsync(book.Id, "Book Updated!");
            }
            else
            {
                return await Result<Guid>.FailAsync("Book not found!");
            }
        }
    }
}