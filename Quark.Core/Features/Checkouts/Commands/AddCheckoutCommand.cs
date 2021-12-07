namespace Quark.Core.Features.Checkouts.Commands;

public class AddCheckoutCommand : IRequest<Result<Guid>>
{
    public Guid Id { get; set; }
    public Guid BookHeaderId { get; set; }
    public string BookBarcode { get; set; }
    public Guid PatronId { get; set; }
    public string PatronRegisterId { get; set; }
    public DateTime? CheckedOutSince { get; set; } = DateTime.Now;
    public DateTime? ExpectedCheckInDate { get; set; } = DateTime.Now.AddDays(15); //TODO: Update the default period
}

internal class AddCheckoutCommandHandler : IRequestHandler<AddCheckoutCommand, Result<Guid>>
{
    private readonly IUnitOfWork<Guid> _unitOfWork;
    private readonly IMapper _mapper;

    public AddCheckoutCommandHandler(IUnitOfWork<Guid> unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<Guid>> Handle(AddCheckoutCommand request, CancellationToken cancellationToken)
    {
        var book = await _unitOfWork.Repository<BookHeader>().Entities.FirstOrDefaultAsync(x => x.Barcode == request.BookBarcode, cancellationToken);
        if(book is null)
        {
            return await Result<Guid>.FailAsync("Invalid Book barcode");
        }
        request.BookHeaderId = book.Id;
        var patron = await _unitOfWork.Repository<Patron>().Entities.FirstOrDefaultAsync(x => x.RegisterId == request.PatronRegisterId, cancellationToken);
        if(patron is null)
        {
            return await Result<Guid>.FailAsync("Invalid Patron Register Id");
        }
        request.PatronId = patron.Id;
        if (await _unitOfWork.Repository<Checkout>().Entities.CountAsync(x => x.PatronId == patron.Id && x.CheckedOutSince.Date == DateTime.Today.Date, cancellationToken) == patron.MultipleCheckoutLimit)
        {
            return await Result<Guid>.FailAsync("Cannot checkout! Day-limit reached!");
        }
        var checkout = _mapper.Map<Checkout>(request);
        await _unitOfWork.Repository<Checkout>().AddAsync(checkout);
        await _unitOfWork.Commit(cancellationToken);
        return await Result<Guid>.SuccessAsync(request.Id, "Checked out successfully");
    }
}