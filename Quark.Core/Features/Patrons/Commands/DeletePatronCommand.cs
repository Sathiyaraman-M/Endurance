namespace Quark.Core.Features.Patrons.Commands;

public class DeletePatronCommand : IRequest<Result<int>>
{
    public int Id { get; set; }
}

public class DeletePatronCommandHandler : IRequestHandler<DeletePatronCommand, Result<int>>
{
    private readonly IUnitOfWork<int> _unitOfWork;

    public DeletePatronCommandHandler(IUnitOfWork<int> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<int>> Handle(DeletePatronCommand request, CancellationToken cancellationToken)
    {
        var patron = await _unitOfWork.Repository<Patron>().Entities.FirstAsync(x => x.Id == request.Id);
        if ((await _unitOfWork.Repository<Checkout>().Entities.Include(x => x.Patron).AnyAsync(x => x.Patron.Id == patron.Id)))
        {
            return await Result<int>.FailAsync("Deletion not allowed!");
        }
        if (patron is not null)
        {
            await _unitOfWork.Repository<Patron>().DeleteAsync(patron);
            await _unitOfWork.Commit(new CancellationToken());
            return await Result<int>.SuccessAsync(patron.Id, "Patron deleted!");
        }
        return await Result<int>.FailAsync("Patron not found!");
    }
}