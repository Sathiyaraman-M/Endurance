using MediatR;
using Quark.Core.Domain.Entities;
using Quark.Core.Interfaces.Repositories;
using Quark.Shared.Wrapper;

namespace Quark.Core.Features.Checkouts.Commands;

public class ExtendCheckoutCommand : IRequest<Result<int>>
{
    public int Id { get; set; }
    public int Days { get; set; }
}

internal class ExtendCheckoutCommandHandler : IRequestHandler<ExtendCheckoutCommand, Result<int>>
{
    private readonly IUnitOfWork<int> _unitOfWork;

    public ExtendCheckoutCommandHandler(IUnitOfWork<int> unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<Result<int>> Handle(ExtendCheckoutCommand request, CancellationToken cancellationToken)
    {
        var checkout = await _unitOfWork.Repository<Checkout>().GetByIdAsync(request.Id);
        if (checkout is not null)
        {
            checkout.ExpectedCheckInDate.AddDays(request.Days);
            await _unitOfWork.Repository<Checkout>().UpdateAsync(checkout);
            await _unitOfWork.Commit(cancellationToken);
            return await Result<int>.SuccessAsync(checkout.Id, $"Extended {request.Days} day(s) successfully!");
        }
        else
        {
            return await Result<int>.FailAsync("Checkout not found!");
        }
    }
}