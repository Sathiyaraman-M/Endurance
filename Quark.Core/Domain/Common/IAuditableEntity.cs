namespace Quark.Core.Domain.Common;

public interface IAuditableEntity<TId> : IAuditableEntity, IEntity<TId> { }

public interface IAuditableEntity
{
    string CreatedBy { get; set; }

    string CreatedByUserId { get; set; }

    DateTime CreatedOn { get; set; }

    string LastModifiedBy { get; set; }

    string LastModifiedByUserId { get; set; }

    DateTime? LastModifiedOn { get; set; }
}