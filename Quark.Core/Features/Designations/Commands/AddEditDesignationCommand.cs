namespace Quark.Core.Features.Designations.Commands;

public class AddEditDesignationCommand : IRequest<Result<Guid>>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}

public class AddEditDesignationCommandHandler : IRequestHandler<AddEditDesignationCommand, Result<Guid>>
{
    private readonly IUnitOfWork<Guid> _unitOfWork;
    private readonly IMapper _mapper;

    public AddEditDesignationCommandHandler(IUnitOfWork<Guid> unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<Guid>> Handle(AddEditDesignationCommand request, CancellationToken cancellationToken)
    {
        var designation = _mapper.Map<Designation>(request);
        if (request.Id == Guid.Empty)
        {
            await _unitOfWork.Repository<Designation>().AddAsync(designation);
            await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllDesignationsCacheKey);
            return await Result<Guid>.SuccessAsync(designation.Id, "Designation Saved!");
        }
        else
        {
            if (await _unitOfWork.Repository<Designation>().Entities.AnyAsync(x => x.Id == request.Id))
            {
                await _unitOfWork.Repository<Designation>().UpdateAsync(designation);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllDesignationsCacheKey);
                return await Result<Guid>.SuccessAsync(request.Id, "Designation Updated!");
            }
            else
            {
                return await Result<Guid>.FailAsync("Designation Not Found!");
            }
        }
    }
}