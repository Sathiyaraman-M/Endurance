namespace Quark.Core.Features.Designations.Commands;

public class DeleteDesignationCommand : IRequest<Result<Guid>>
{
    public Guid Id { get; set; }
}

public class DeleteDesignationCommandHandler : IRequestHandler<DeleteDesignationCommand, Result<Guid>>
{
    private readonly IUnitOfWork<Guid> _unitOfWork;
    private readonly IDesignationRepository _designationRepository;

    public DeleteDesignationCommandHandler(IUnitOfWork<Guid> unitOfWork, IDesignationRepository designationRepository)
    {
        _unitOfWork = unitOfWork;
        _designationRepository = designationRepository;
    }

    public async Task<Result<Guid>> Handle(DeleteDesignationCommand request, CancellationToken cancellationToken)
    {
        if (await _designationRepository.IsDesignationUsed(request.Id))
        {
            return await Result<Guid>.FailAsync("Deletion not allowed");
        }
        var designation = await _unitOfWork.Repository<Designation>().GetByIdAsync(request.Id);
        if (designation is not null)
        {
            await _unitOfWork.Repository<Designation>().DeleteAsync(designation);
            await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllDesignationsCacheKey);
            return await Result<Guid>.SuccessAsync(request.Id, "Designation deleted!");
        }
        else
        {
            return await Result<Guid>.FailAsync("Designation Not Found!");
        }
    }
}