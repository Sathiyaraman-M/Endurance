namespace Quark.Core.Features.Checkouts.Commands;

public class ExtendCheckoutCommand : IRequest<Result<Guid>>
{
    public Guid Id { get; set; }
    public int Days { get; set; }
}

internal class ExtendCheckoutCommandHandler : IRequestHandler<ExtendCheckoutCommand, Result<Guid>>
{
    private readonly IUnitOfWork<Guid> _unitOfWork;

    public ExtendCheckoutCommandHandler(IUnitOfWork<Guid> unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<Result<Guid>> Handle(ExtendCheckoutCommand request, CancellationToken cancellationToken)
    {
        var checkout = await _unitOfWork.Repository<Checkout>().GetByIdAsync(request.Id);
        if (checkout is not null)
        {
            checkout.ExpectedCheckInDate.AddDays(request.Days);
            await _unitOfWork.Repository<Checkout>().UpdateAsync(checkout);
            await _unitOfWork.Commit(cancellationToken);
            return await Result<Guid>.SuccessAsync(checkout.Id, $"Extended {request.Days} day(s) successfully!");
        }
        else
        {
            return await Result<Guid>.FailAsync("Checkout not found!");
        }
    }
}