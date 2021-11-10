using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Quark.Core.Domain.Entities;
using Quark.Core.Interfaces.Repositories;
using Quark.Shared.Wrapper;

namespace Quark.Core.Features.Checkouts.Commands;

public class AddCheckoutCommand : IRequest<Result<int>>
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public string BookBarcode { get; set; }
    public int PatronId { get; set; }
    public string PatronRegisterId { get; set; }
    public DateTime? CheckedOutSince { get; set; } = DateTime.Now;
    public DateTime? ExpectedCheckInDate { get; set; } = DateTime.Now.AddDays(15); //TODO: Update the default period
}

internal class AddCheckoutCommandHandler : IRequestHandler<AddCheckoutCommand, Result<int>>
{
    private readonly IUnitOfWork<int> _unitOfWork;
    private readonly IMapper _mapper;

    public AddCheckoutCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<int>> Handle(AddCheckoutCommand request, CancellationToken cancellationToken)
    {
        var book = await _unitOfWork.Repository<Book>().Entities.FirstOrDefaultAsync(x => x.Barcode == request.BookBarcode);
        if(book is null)
        {
            return await Result<int>.FailAsync("Invalid Book barcode");
        }
        if(await _unitOfWork.Repository<Checkout>().Entities.CountAsync(x => x.BookId == request.BookId && x.CheckedOutUntil == null) == book.Copies)
        {
            return await Result<int>.FailAsync("Book copies not available for checkout");
        }
        request.BookId = book.Id;
        var patron = await _unitOfWork.Repository<Patron>().Entities.FirstOrDefaultAsync(x => x.RegisterId == request.PatronRegisterId);
        if(patron is null)
        {
            return await Result<int>.FailAsync("Invalid Patron Register Id");
        }
        request.PatronId = patron.Id;
        if (await _unitOfWork.Repository<Checkout>().Entities.CountAsync(x => x.PatronId == patron.Id && x.CheckedOutSince.Date == DateTime.Today.Date) == patron.MultipleCheckoutLimit)
        {
            return await Result<int>.FailAsync("Cannot checkout! Day-limit reached!");
        }
        var checkout = _mapper.Map<Checkout>(request);
        await _unitOfWork.Repository<Checkout>().AddAsync(checkout);
        await _unitOfWork.Commit(cancellationToken);
        return await Result<int>.SuccessAsync(request.Id, "Checked out successfully");
    }
}