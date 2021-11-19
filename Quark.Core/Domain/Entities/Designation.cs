namespace Quark.Core.Domain.Entities;

public class Designation : AuditableEntity<int>
{
    public string Name { get; set; }
}