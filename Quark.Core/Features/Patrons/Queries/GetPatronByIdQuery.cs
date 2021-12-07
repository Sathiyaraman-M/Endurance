namespace Quark.Core.Features.Patrons.Queries;

public class GetPatronByIdQuery : IRequest<Result<PatronResponse>>
{
    public Guid Id { get; set; }
    public GetPatronByIdQuery(Guid id) => Id = id;
}

internal class GetPatronByIdQueryHandler : IRequestHandler<GetPatronByIdQuery, Result<PatronResponse>>
{
    private readonly IUnitOfWork<Guid> _unitOfWork;
    private readonly IMapper _mapper;

    public GetPatronByIdQueryHandler(IUnitOfWork<Guid> unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<PatronResponse>> Handle(GetPatronByIdQuery request, CancellationToken cancellationToken)
    {
        var patron = _mapper.Map<PatronResponse>(await _unitOfWork.Repository<Patron>().GetByIdAsync(request.Id));
        patron.CheckoutsCount = await _unitOfWork.Repository<Checkout>().Entities.CountAsync(x => x.PatronId == request.Id);
        patron.CheckoutsPending = await _unitOfWork.Repository<Checkout>().Entities.CountAsync(x => x.PatronId == request.Id && !x.CheckedOutUntil.HasValue);
        return await Result<PatronResponse>.SuccessAsync(patron);
    }
}