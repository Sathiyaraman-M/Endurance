using Microsoft.AspNetCore.Identity;
using Quark.Core.Domain.Common;

namespace Quark.Infrastructure.Models.Identity;

public class ApplicationRoleClaim : IdentityRoleClaim<string>, IAuditableEntity<int>
{
    public ApplicationRoleClaim() : base()
    {

    }

    public ApplicationRoleClaim(string roleClaimDescription = null, string roleClaimGroup = null) : base()
    {
        Description = roleClaimDescription;
        Group = roleClaimGroup;
    }

    public string Description { get; set; }
    public string Group { get; set; }

    public virtual ApplicationRole Role { get; set; }

    public string CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public string LastModifiedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; }
    public string CreatedByUserId { get; set; }
    public string LastModifiedByUserId { get; set; }
}