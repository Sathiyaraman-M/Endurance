namespace Quark.Core.Domain.Entities;

public class BookHeader : AuditableEntity<Guid>
{
    public string Barcode { get; set; }
    public string Condition { get; set; }
    public Book Book { get; set; }
    public Guid BookId { get; set; }
}