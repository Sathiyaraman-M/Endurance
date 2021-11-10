using AutoMapper;
using LazyCache;
using MediatR;
using Quark.Core.Domain.Entities;
using Quark.Core.Interfaces.Repositories;
using Quark.Core.Responses;
using Quark.Shared.Constants;
using Quark.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Quark.Core.Features.Designations.Queries;

public class GetAllDesignationsQuery : IRequest<Result<List<DesignationResponse>>>
{

}

public class GetAllDesignationQueryHandler : IRequestHandler<GetAllDesignationsQuery, Result<List<DesignationResponse>>>
{
    private readonly IUnitOfWork<int> _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IAppCache _appCache;

    public GetAllDesignationQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache appCache)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _appCache = appCache;
    }

    public async Task<Result<List<DesignationResponse>>> Handle(GetAllDesignationsQuery request, CancellationToken cancellationToken)
    {
        Func<Task<List<Designation>>> getAllDesignations = () => _unitOfWork.Repository<Designation>().GetAllAsync();
        var designationsList = await _appCache.GetOrAddAsync(ApplicationConstants.Cache.GetAllDesignationsCacheKey, getAllDesignations);
        var mappedDesignations = _mapper.Map<List<DesignationResponse>>(designationsList);
        return await Result<List<DesignationResponse>>.SuccessAsync(mappedDesignations);
    }
}