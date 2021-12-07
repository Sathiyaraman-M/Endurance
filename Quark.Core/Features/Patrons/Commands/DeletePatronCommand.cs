namespace Quark.Core.Features.Patrons.Commands;

public class DeletePatronCommand : IRequest<Result<Guid>>
{
    public Guid Id { get; set; }
}

public class DeletePatronCommandHandler : IRequestHandler<DeletePatronCommand, Result<Guid>>
{
    private readonly IUnitOfWork<Guid> _unitOfWork;

    public DeletePatronCommandHandler(IUnitOfWork<Guid> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(DeletePatronCommand request, CancellationToken cancellationToken)
    {
        var patron = await _unitOfWork.Repository<Patron>().Entities.FirstAsync(x => x.Id == request.Id);
        if ((await _unitOfWork.Repository<Checkout>().Entities.Include(x => x.Patron).AnyAsync(x => x.Patron.Id == patron.Id)))
        {
            return await Result<Guid>.FailAsync("Deletion not allowed!");
        }
        if (patron is not null)
        {
            await _unitOfWork.Repository<Patron>().DeleteAsync(patron);
            await _unitOfWork.Commit(new CancellationToken());
            return await Result<Guid>.SuccessAsync(patron.Id, "Patron deleted!");
        }
        return await Result<Guid>.FailAsync("Patron not found!");
    }
}