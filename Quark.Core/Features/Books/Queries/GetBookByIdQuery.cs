using AutoMapper;
using MediatR;
using Quark.Core.Domain.Entities;
using Quark.Core.Interfaces.Repositories;
using Quark.Core.Responses;
using Quark.Shared.Wrapper;

namespace Quark.Core.Features.Books.Queries;

public class GetBookByIdQuery : IRequest<Result<BookResponse>>
{
    public int Id { get; set; }

    public GetBookByIdQuery(int id) => Id = id;
}

internal class GetBookBydIdQueryHandler : IRequestHandler<GetBookByIdQuery, Result<BookResponse>>
{
    private readonly IUnitOfWork<int> _unitOfWork;
    private readonly IMapper _mapper;

    public GetBookBydIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<BookResponse>> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
    {
        var book = _mapper.Map<BookResponse>(await _unitOfWork.Repository<Book>().GetByIdAsync(request.Id));
        return await Result<BookResponse>.SuccessAsync(book);
    }
}