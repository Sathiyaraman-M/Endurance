using MediatR;
using Microsoft.EntityFrameworkCore;
using Quark.Core.Domain.Entities;
using Quark.Core.Interfaces.Repositories;
using Quark.Core.Responses;
using Quark.Shared.Wrapper;

namespace Quark.Core.Features.Checkouts.Queries;

public class GetCheckoutByIdQuery : IRequest<Result<CheckoutResponse>>
{
    public int Id { get; set; }
    public GetCheckoutByIdQuery(int id) => Id = id;
}

internal class GetCheckoutBydIdQueryHandler : IRequestHandler<GetCheckoutByIdQuery, Result<CheckoutResponse>>
{
    private readonly IUnitOfWork<int> _unitOfWork;
    public GetCheckoutBydIdQueryHandler(IUnitOfWork<int> unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<Result<CheckoutResponse>> Handle(GetCheckoutByIdQuery request, CancellationToken cancellationToken)
    {
        //Expression<Func<Checkout, CheckoutResponse>> expression = e => new CheckoutResponse
        //{
        //    Id = e.Id,
        //    BookId = e.BookId,
        //    BookName = e.Book.Name,
        //    DeweyIndex = e.Book.DeweyIndex,
        //    BookBarcode = e.Book.Barcode,
        //    PatronId = e.PatronId,
        //    PatronRegisterId = e.Patron.RegisterId,
        //    PatronName = e.Patron.FirstName + " " + e.Patron.LastName,
        //    CheckedOutSince = e.CheckedOutSince,
        //    ExpectedCheckInDate = e.ExpectedCheckInDate,
        //    CheckedOutUntil = e.CheckedOutUntil
        //};
        //var Checkout = _mapper.Map<CheckoutResponse>(await _unitOfWork.Repository<Checkout>().GetByIdAsync(request.Id));
        //var checkout = await _unitOfWork.Repository<Checkout>().Entities.Select(expression).FirstAsync(x => x.Id == request.Id);
        var checkout = await _unitOfWork.Repository<Checkout>().Entities.Include(x => x.Book).Include(x => x.Patron).FirstAsync(x => x.Id == request.Id);
        var checkoutResponse = new CheckoutResponse
        {
            Id = checkout.Id,
            BookId = checkout.BookId,
            BookName = checkout.Book.Name,
            DeweyIndex = checkout.Book.DeweyIndex,
            BookBarcode = checkout.Book.Barcode,
            PatronId = checkout.PatronId,
            PatronRegisterId = checkout.Patron.RegisterId,
            PatronName = checkout.Patron.FirstName + " " + checkout.Patron.LastName,
            CheckedOutSince = checkout.CheckedOutSince,
            ExpectedCheckInDate = checkout.ExpectedCheckInDate,
            CheckedOutUntil = checkout.CheckedOutUntil
        };
        return await Result<CheckoutResponse>.SuccessAsync(checkoutResponse);
    }
}