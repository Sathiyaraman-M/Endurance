namespace Quark.Core.Domain.Common;

public class AuditableEntity<TId> : IAuditableEntity<TId>
{
    public TId Id { get; set; }

    public string CreatedBy { get; set; }

    public string CreatedByUserId { get; set; }

    public DateTime CreatedOn { get; set; }

    public string LastModifiedBy { get; set; }

    public string LastModifiedByUserId { get; set; }

    public DateTime? LastModifiedOn { get; set; }
}