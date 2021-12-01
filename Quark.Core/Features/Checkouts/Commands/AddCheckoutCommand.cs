namespace Quark.Core.Features.Checkouts.Commands;

public class AddCheckoutCommand : IRequest<Result<int>>
{
    public int Id { get; set; }
    public Guid BookHeaderId { get; set; }
    public string BookBarcode { get; set; }
    public int PatronId { get; set; }
    public string PatronRegisterId { get; set; }
    public DateTime? CheckedOutSince { get; set; } = DateTime.Now;
    public DateTime? ExpectedCheckInDate { get; set; } = DateTime.Now.AddDays(15); //TODO: Update the default period
}

internal class AddCheckoutCommandHandler : IRequestHandler<AddCheckoutCommand, Result<int>>
{
    private readonly IUnitOfWork<int> _unitOfWorkInt;
    private readonly IUnitOfWork<Guid> _unitOfWorkGuid;
    private readonly IMapper _mapper;

    public AddCheckoutCommandHandler(IUnitOfWork<int> unitOfWorkInt, IUnitOfWork<Guid> unitOfWorkGuid, IMapper mapper)
    {
        _unitOfWorkInt = unitOfWorkInt;
        _unitOfWorkGuid = unitOfWorkGuid;
        _mapper = mapper;
    }

    public async Task<Result<int>> Handle(AddCheckoutCommand request, CancellationToken cancellationToken)
    {
        var book = await _unitOfWorkGuid.Repository<BookHeader>().Entities.FirstOrDefaultAsync(x => x.Barcode == request.BookBarcode);
        if(book is null)
        {
            return await Result<int>.FailAsync("Invalid Book barcode");
        }
        request.BookHeaderId = book.Id;
        var patron = await _unitOfWorkInt.Repository<Patron>().Entities.FirstOrDefaultAsync(x => x.RegisterId == request.PatronRegisterId);
        if(patron is null)
        {
            return await Result<int>.FailAsync("Invalid Patron Register Id");
        }
        request.PatronId = patron.Id;
        if (await _unitOfWorkInt.Repository<Checkout>().Entities.CountAsync(x => x.PatronId == patron.Id && x.CheckedOutSince.Date == DateTime.Today.Date) == patron.MultipleCheckoutLimit)
        {
            return await Result<int>.FailAsync("Cannot checkout! Day-limit reached!");
        }
        var checkout = _mapper.Map<Checkout>(request);
        await _unitOfWorkInt.Repository<Checkout>().AddAsync(checkout);
        await _unitOfWorkInt.Commit(cancellationToken);
        return await Result<int>.SuccessAsync(request.Id, "Checked out successfully");
    }
}