using Quark.Core.Responses.Audit;
using Quark.Infrastructure.Models.Audit;
using System.Linq.Expressions;

namespace Quark.Infrastructure.Services;

public class AuditService : IAuditService
{
    private readonly LibraryDbContext _dbContext;

    public AuditService(LibraryDbContext dbContext) => _dbContext = dbContext;

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
        if (userId == "")
        {
            var auditTrails = await _dbContext.AuditTrails.Where(x => x.UserId == userId).Select(expression).OrderByDescending(a => a.Id).Take(250).ToListAsync();
            return await Result<List<AuditResponse>>.SuccessAsync(auditTrails);
        }
        else
        {
            var auditTrails = await _dbContext.AuditTrails.Select(expression).OrderByDescending(a => a.Id).Take(250).ToListAsync();
            return await Result<List<AuditResponse>>.SuccessAsync(auditTrails);
        }
    }
}