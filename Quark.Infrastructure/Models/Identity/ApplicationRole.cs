namespace Quark.Infrastructure.Models.Identity;

public class ApplicationRole : IdentityRole, IAuditableEntity<string>
{
    public ApplicationRole() : base()
    {
        RoleClaims = new HashSet<ApplicationRoleClaim>();
    }

    public ApplicationRole(string roleName, string roleDescription = null) : base(roleName)
    {
        RoleClaims = new HashSet<ApplicationRoleClaim>();
        Description = roleDescription;
    }

    public string Description { get; set; }
    public virtual ICollection<ApplicationRoleClaim> RoleClaims { get; set; }

    public string CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public string LastModifiedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; }
    public string CreatedByUserId { get; set; }
    public string LastModifiedByUserId { get; set; }
}