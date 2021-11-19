namespace Quark.Core.Features.Designations.Commands;

public class DeleteDesignationCommand : IRequest<Result<int>>
{
    public int Id { get; set; }
}

public class DeleteDesignationCommandHandler : IRequestHandler<DeleteDesignationCommand, Result<int>>
{
    private readonly IUnitOfWork<int> _unitOfWork;
    private readonly IDesignationRepository _designationRepository;

    public DeleteDesignationCommandHandler(IUnitOfWork<int> unitOfWork, IDesignationRepository designationRepository)
    {
        _unitOfWork = unitOfWork;
        _designationRepository = designationRepository;
    }

    public async Task<Result<int>> Handle(DeleteDesignationCommand request, CancellationToken cancellationToken)
    {
        if (await _designationRepository.IsDesignationUsed(request.Id))
        {
            return await Result<int>.FailAsync("Deletion not allowed");
        }
        var designation = await _unitOfWork.Repository<Designation>().GetByIdAsync(request.Id);
        if (designation is not null)
        {
            await _unitOfWork.Repository<Designation>().DeleteAsync(designation);
            await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllDesignationsCacheKey);
            return await Result<int>.SuccessAsync(request.Id, "Designation deleted!");
        }
        else
        {
            return await Result<int>.FailAsync("Designation Not Found!");
        }
    }
}