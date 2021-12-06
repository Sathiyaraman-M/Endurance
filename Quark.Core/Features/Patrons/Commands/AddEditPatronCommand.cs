namespace Quark.Core.Features.Patrons.Commands;

public class AddEditPatronCommand : IRequest<Result<Guid>>
{
    public Guid Id { get; set; }
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

public class AddEditPatronCommandHandler : IRequestHandler<AddEditPatronCommand, Result<Guid>>
{
    private readonly IUnitOfWork<Guid> _unitOfWork;
    private readonly IMapper _mapper;

    public AddEditPatronCommandHandler(IUnitOfWork<Guid> unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<Guid>> Handle(AddEditPatronCommand request, CancellationToken cancellationToken)
    {
        if (await _unitOfWork.Repository<Patron>().Entities.Where(p => p.Id != request.Id).AnyAsync(x => x.RegisterId == request.RegisterId, cancellationToken))
        {
            return await Result<Guid>.FailAsync("Register number already exists!");
        }
        var patron = _mapper.Map<Patron>(request);
        if (request.Id == Guid.Empty)
        {
            await _unitOfWork.Repository<Patron>().AddAsync(patron);
            await _unitOfWork.Commit(cancellationToken);
            return await Result<Guid>.SuccessAsync(patron.Id, "Patron added!");
        }
        else
        {
            if (await _unitOfWork.Repository<Patron>().GetByIdAsync(request.Id) is not null)
            {
                await _unitOfWork.Repository<Patron>().UpdateAsync(patron);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<Guid>.SuccessAsync(patron.Id, "Patron updated!");
            }
            else
            {
                return await Result<Guid>.FailAsync("Patron not found!");
            }
        }
    }
}