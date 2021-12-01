namespace Quark.Core.Features.Checkouts.Commands;

public class CheckInBookCommand : IRequest<Result<int>>
{
    public string BookBarcode { get; set; }
    public DateTime? CheckInDate { get; set; } = DateTime.Now;
}

internal class CheckInBookCommandHandler : IRequestHandler<CheckInBookCommand, Result<int>>
{
    private readonly IUnitOfWork<int> _unitOfWork;
    private readonly IUnitOfWork<Guid> _unitOfWorkGuid;

    public CheckInBookCommandHandler(IUnitOfWork<int> unitOfWork, IUnitOfWork<Guid> unitOfWorkGuid)
    {
        _unitOfWork = unitOfWork;
        _unitOfWorkGuid = unitOfWorkGuid;
    }

    public async Task<Result<int>> Handle(CheckInBookCommand request, CancellationToken cancellationToken)
    {
        var book = await _unitOfWorkGuid.Repository<BookHeader>().Entities.FirstOrDefaultAsync(x => x.Barcode == request.BookBarcode);
        var checkout = await _unitOfWork.Repository<Checkout>().Entities.FirstOrDefaultAsync(x => !x.CheckedOutUntil.HasValue && x.BookHeaderId == book.Id);
        if (checkout is not null)
        {
            checkout.CheckedOutUntil = request.CheckInDate;
            //TODO: Update Patron Fine Charges here
            await _unitOfWork.Repository<Checkout>().UpdateAsync(checkout);
            await _unitOfWork.Commit(cancellationToken);
            return await Result<int>.SuccessAsync(checkout.Id, "Book checked in successfully");
        }
        else
        {
            return await Result<int>.FailAsync("Checkout not found!");
        }
    }
}