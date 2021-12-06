namespace Quark.Core.Features.Checkouts.Commands;

public class DeleteCheckoutCommand : IRequest<Result<Guid>>
{
    public Guid Id { get; set; }

    public DeleteCheckoutCommand(Guid id)
    {
        Id = id;
    }
}

internal class DeleteCheckoutCommandHandler : IRequestHandler<DeleteCheckoutCommand, Result<Guid>>
{
    private readonly IUnitOfWork<Guid> _unitOfWork;

    public DeleteCheckoutCommandHandler(IUnitOfWork<Guid> unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<Result<Guid>> Handle(DeleteCheckoutCommand request, CancellationToken cancellationToken)
    {
        var Checkout = await _unitOfWork.Repository<Checkout>().GetByIdAsync(request.Id);
        if (Checkout is not null)
        {
            await _unitOfWork.Repository<Checkout>().DeleteAsync(Checkout);
            await _unitOfWork.Commit(cancellationToken);
            return await Result<Guid>.SuccessAsync(request.Id, "Checkout deleted!");
        }
        else
        {
            return await Result<Guid>.FailAsync("Checkout Not Found!");
        }
    }
}