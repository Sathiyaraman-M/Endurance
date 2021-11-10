using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Quark.Core.Interfaces.Services;
using Quark.Core.Responses.Audit;
using Quark.Infrastructure.DbContexts;
using Quark.Infrastructure.Models.Audit;
using Quark.Shared.Wrapper;
using System.Linq.Expressions;

namespace Quark.Infrastructure.Services;

public class AuditService : IAuditService
{
    private readonly LibraryDbContext _dbContext;
    private readonly IMapper _mapper;

    public AuditService(LibraryDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<IResult<List<AuditResponse>>> GetCurrentUserTrailsAsync(string userId = "")
    {
        Expression<Func<Audit, AuditResponse>> expression = e => new AuditResponse
        {
            Id = e.Id,
            UserId = e.UserId,
            Type = e.Type,
            TableName = e.TableName,
            DateTime = e.DateTime,
            OldValues = e.OldValues,
            NewValues = e.NewValues,
            AffectedColumns = e.AffectedColumns,
            PrimaryKey = e.PrimaryKey
        };
        //var trails = await _dbContext.AuditTrails.Where(x => x.UserId == userId).OrderByDescending(a => a.Id).Take(250).ToListAsync();
        var auditTrails = await _dbContext.AuditTrails.Where(x => x.UserId == userId).Select(expression).OrderByDescending(a => a.Id).Take(250).ToListAsync();
        return await Result<List<AuditResponse>>.SuccessAsync(auditTrails);
    }
}