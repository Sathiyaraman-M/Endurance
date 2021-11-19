namespace Quark.Core.Features.Patrons.Commands;

public class AddEditPatronCommand : IRequest<Result<int>>
{
    public int Id { get; set; }
    public string RegisterId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string Email { get; set; }
    public string Mobile { get; set; }
    public DateTime? Issued { get; set; }
    public int MultipleCheckoutLimit { get; set; }
}

public class AddEditPatronCommandHandler : IRequestHandler<AddEditPatronCommand, Result<int>>
{
    private readonly IUnitOfWork<int> _unitOfWork;
    private readonly IMapper _mapper;

    public AddEditPatronCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<int>> Handle(AddEditPatronCommand request, CancellationToken cancellationToken)
    {
        if (await _unitOfWork.Repository<Patron>().Entities.Where(p => p.Id != request.Id).AnyAsync(x => x.RegisterId == request.RegisterId, cancellationToken))
        {
            return await Result<int>.FailAsync("Register number already exists!");
        }
        var patron = _mapper.Map<Patron>(request);
        if (request.Id == 0)
        {
            await _unitOfWork.Repository<Patron>().AddAsync(patron);
            await _unitOfWork.Commit(cancellationToken);
            return await Result<int>.SuccessAsync(patron.Id, "Patron added!");
        }
        else
        {
            if (await _unitOfWork.Repository<Patron>().GetByIdAsync(request.Id) is not null)
            {
                await _unitOfWork.Repository<Patron>().UpdateAsync(patron);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(patron.Id, "Patron updated!");
            }
            else
            {
                return await Result<int>.FailAsync("Patron not found!");
            }
        }
    }
}