namespace Quark.Core.Features.Designations.Commands;

public class AddEditDesignationCommand : IRequest<Result<int>>
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class AddEditDesignationCommandHandler : IRequestHandler<AddEditDesignationCommand, Result<int>>
{
    private readonly IUnitOfWork<int> _unitOfWork;
    private readonly IMapper _mapper;

    public AddEditDesignationCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<int>> Handle(AddEditDesignationCommand request, CancellationToken cancellationToken)
    {
        var designation = _mapper.Map<Designation>(request);
        if (request.Id == 0)
        {
            await _unitOfWork.Repository<Designation>().AddAsync(designation);
            await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllDesignationsCacheKey);
            return await Result<int>.SuccessAsync(designation.Id, "Designation Saved!");
        }
        else
        {
            if (await _unitOfWork.Repository<Designation>().Entities.AnyAsync(x => x.Id == request.Id))
            {
                await _unitOfWork.Repository<Designation>().UpdateAsync(designation);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllDesignationsCacheKey);
                return await Result<int>.SuccessAsync(request.Id, "Designation Updated!");
            }
            else
            {
                return await Result<int>.FailAsync("Designation Not Found!");
            }
        }
    }
}