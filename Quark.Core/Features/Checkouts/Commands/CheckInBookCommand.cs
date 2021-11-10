using MediatR;
using Quark.Core.Domain.Entities;
using Quark.Core.Interfaces.Repositories;
using Quark.Shared.Wrapper;

namespace Quark.Core.Features.Checkouts.Commands;

public class CheckInBookCommand : IRequest<Result<int>>
{
    public int Id { get; set; }
    public DateTime CheckInDate { get; set; } = DateTime.Now;
}

internal class CheckInBookCommandHandler : IRequestHandler<CheckInBookCommand, Result<int>>
{
    private readonly IUnitOfWork<int> _unitOfWork;

    public CheckInBookCommandHandler(IUnitOfWork<int> unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<Result<int>> Handle(CheckInBookCommand request, CancellationToken cancellationToken)
    {
        var checkout = await _unitOfWork.Repository<Checkout>().GetByIdAsync(request.Id);
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