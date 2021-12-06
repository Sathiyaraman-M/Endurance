namespace Quark.Core.Domain.Entities;

public class Designation : AuditableEntity<Guid>
{
    public string Name { get; set; }
}