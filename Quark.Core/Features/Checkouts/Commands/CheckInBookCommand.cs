namespace Quark.Core.Features.Checkouts.Commands;

public class CheckInBookCommand : IRequest<Result<Guid>>
{
    public string BookBarcode { get; set; }
    public DateTime? CheckInDate { get; set; } = DateTime.Now;
}

internal class CheckInBookCommandHandler : IRequestHandler<CheckInBookCommand, Result<Guid>>
{
    private readonly IUnitOfWork<Guid> _unitOfWork;

    public CheckInBookCommandHandler(IUnitOfWork<Guid> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CheckInBookCommand request, CancellationToken cancellationToken)
    {
        var book = await _unitOfWork.Repository<BookHeader>().Entities.FirstOrDefaultAsync(x => x.Barcode == request.BookBarcode, cancellationToken);
        var checkout = await _unitOfWork.Repository<Checkout>().Entities.FirstOrDefaultAsync(x => !x.CheckedOutUntil.HasValue && x.BookHeaderId == book.Id, cancellationToken);
        if (checkout is not null)
        {
            checkout.CheckedOutUntil = request.CheckInDate;
            //TODO: Update Patron Fine Charges here
            await _unitOfWork.Repository<Checkout>().UpdateAsync(checkout);
            await _unitOfWork.Commit(cancellationToken);
            return await Result<Guid>.SuccessAsync(checkout.Id, "Book checked in successfully");
        }
        else
        {
            return await Result<Guid>.FailAsync("Checkout not found!");
        }
    }
}