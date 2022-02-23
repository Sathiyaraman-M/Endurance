using Quark.Core.Configurations;

namespace Quark.Core.Features.Patrons.Commands;

public class UpdatePatronDuesCommand : IRequest<IResult>
{
    public UpdatePatronDuesCommand(Guid id, LibrarySettings settings)
    {
        Id = id;
        Settings = settings;
    }

    public Guid Id { get; set; }
    public LibrarySettings Settings { get; }
}

internal class UpdatePatronDuesCommandHandler : IRequestHandler<UpdatePatronDuesCommand, IResult>
{
    private readonly IUnitOfWork<Guid> _unitOfWork;

    public UpdatePatronDuesCommandHandler(IUnitOfWork<Guid> unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<IResult> Handle(UpdatePatronDuesCommand request, CancellationToken cancellationToken)
    {
        var patron = await _unitOfWork.Repository<Patron>().GetByIdAsync(request.Id);
        var checkouts = await _unitOfWork.Repository<Checkout>().Entities.Where(x => !x.CheckedOutUntil.HasValue && x.PatronId == request.Id).ToListAsync(cancellationToken);
        foreach (var checkout in checkouts)
        {
            patron.CurrentFees += (DateTime.Today - checkout.ExpectedCheckInDate).Days * request.Settings.CheckInDelayFinePerDay;
        }
        await _unitOfWork.Repository<Patron>().UpdateAsync(patron);
        await _unitOfWork.Commit(cancellationToken);
        return await Result.SuccessAsync();
    }
}