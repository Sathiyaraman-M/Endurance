namespace Quark.Core.Features.Checkouts.Commands;

public class DeleteCheckoutCommand : IRequest<Result<int>>
{
    public int Id { get; set; }

    public DeleteCheckoutCommand(int id)
    {
        Id = id;
    }
}

internal class DeleteCheckoutCommandHandler : IRequestHandler<DeleteCheckoutCommand, Result<int>>
{
    private readonly IUnitOfWork<int> _unitOfWork;

    public DeleteCheckoutCommandHandler(IUnitOfWork<int> unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<Result<int>> Handle(DeleteCheckoutCommand request, CancellationToken cancellationToken)
    {
        var Checkout = await _unitOfWork.Repository<Checkout>().GetByIdAsync(request.Id);
        if (Checkout is not null)
        {
            await _unitOfWork.Repository<Checkout>().DeleteAsync(Checkout);
            await _unitOfWork.Commit(cancellationToken);
            return await Result<int>.SuccessAsync(request.Id, "Checkout deleted!");
        }
        else
        {
            return await Result<int>.FailAsync("Checkout Not Found!");
        }
    }
}