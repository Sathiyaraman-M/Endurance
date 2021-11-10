using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Quark.Core.Interfaces.Services;
using Quark.Core.Interfaces.Services.Identity;
using Quark.Core.Requests.Identity;
using Quark.Core.Responses.Identity;
using Quark.Infrastructure.DbContexts;
using Quark.Infrastructure.Models.Identity;
using Quark.Shared.Wrapper;

namespace Quark.Infrastructure.Services.Identity;

public class RoleClaimService : IRoleClaimService
{
    private readonly LibraryDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public RoleClaimService(LibraryDbContext dbContext, IMapper mapper, ICurrentUserService currentUserService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<Result<List<RoleClaimResponse>>> GetAllAsync()
    {
        var roleClaims = await _dbContext.RoleClaims.ToListAsync();
        var roleClaimsResponse = _mapper.Map<List<RoleClaimResponse>>(roleClaims);
        return await Result<List<RoleClaimResponse>>.SuccessAsync(roleClaimsResponse);
    }

    public async Task<int> GetCountAsync()
    {
        return await _dbContext.RoleClaims.CountAsync();
    }

    public async Task<Result<RoleClaimResponse>> GetByIdAsync(int id)
    {
        var roleClaim = await _dbContext.RoleClaims.SingleOrDefaultAsync(x => x.Id == id);
        var roleClaimResponse = _mapper.Map<RoleClaimResponse>(roleClaim);
        return await Result<RoleClaimResponse>.SuccessAsync(roleClaimResponse);
    }

    public async Task<Result<List<RoleClaimResponse>>> GetAllByRoleIdAsync(string roleId)
    {
        var roleClaims = await _dbContext.RoleClaims
            .Include(x => x.Role)
            .Where(x => x.RoleId == roleId)
            .ToListAsync();
        var roleClaimsResponse = _mapper.Map<List<RoleClaimResponse>>(roleClaims);
        return await Result<List<RoleClaimResponse>>.SuccessAsync(roleClaimsResponse);
    }

    public async Task<Result<string>> SaveAsync(RoleClaimRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.RoleId))
        {
            return await Result<string>.FailAsync("Role is required.");
        }

        if (request.Id == 0)
        {
            var existingRoleClaim =
                await _dbContext.RoleClaims
                    .SingleOrDefaultAsync(x =>
                        x.RoleId == request.RoleId && x.ClaimType == request.Type && x.ClaimValue == request.Value);
            if (existingRoleClaim != null)
            {
                return await Result<string>.FailAsync("Similar Role Claim already exists.");
            }
            var roleClaim = _mapper.Map<ApplicationRoleClaim>(request);
            await _dbContext.RoleClaims.AddAsync(roleClaim);
            await _dbContext.SaveChangesAsync(_currentUserService.UserId);
            return await Result<string>.SuccessAsync(string.Format("Role Claim {0} created.", request.Value));
        }
        else
        {
            var existingRoleClaim =
                await _dbContext.RoleClaims
                    .Include(x => x.Role)
                    .SingleOrDefaultAsync(x => x.Id == request.Id);
            if (existingRoleClaim == null)
            {
                return await Result<string>.SuccessAsync("Role Claim does not exist.");
            }
            else
            {
                existingRoleClaim.ClaimType = request.Type;
                existingRoleClaim.ClaimValue = request.Value;
                existingRoleClaim.Group = request.Group;
                existingRoleClaim.Description = request.Description;
                existingRoleClaim.RoleId = request.RoleId;
                _dbContext.RoleClaims.Update(existingRoleClaim);
                await _dbContext.SaveChangesAsync(_currentUserService.UserId);
                return await Result<string>.SuccessAsync(string.Format("Role Claim {0} for Role {1} updated.", request.Value, existingRoleClaim.Role.Name));
            }
        }
    }

    public async Task<Result<string>> DeleteAsync(int id)
    {
        var existingRoleClaim = await _dbContext.RoleClaims
            .Include(x => x.Role)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (existingRoleClaim != null)
        {
            _dbContext.RoleClaims.Remove(existingRoleClaim);
            await _dbContext.SaveChangesAsync(_currentUserService.UserId);
            return await Result<string>.SuccessAsync(string.Format("Role Claim {0} for {1} Role deleted.", existingRoleClaim.ClaimValue, existingRoleClaim.Role.Name));
        }
        else
        {
            return await Result<string>.FailAsync("Role Claim does not exist.");
        }
    }
}