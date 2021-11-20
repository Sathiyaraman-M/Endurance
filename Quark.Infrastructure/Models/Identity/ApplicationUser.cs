using System.ComponentModel.DataAnnotations.Schema;

namespace Quark.Infrastructure.Models.Identity;

public class ApplicationUser : IdentityUser<string>, IAuditableEntity<string>
{
    public string FullName { get; set; }

    [Column(TypeName = "text")]
    public string ProfilePictureDataUrl { get; set; }

    public int DesignationId { get; set; }
    //public virtual Designation Designation { get; set; }

    public string CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public string LastModifiedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; }

    public DateTime? DeletedOn { get; set; }
    public bool IsActive { get; set; }

    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
    public string CreatedByUserId { get; set; }
    public string LastModifiedByUserId { get; set; }
}