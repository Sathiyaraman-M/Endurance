namespace Quark.Core.Domain.Entities;

public class Book : AuditableEntity<Guid>
{
    public string Name { get; set; }
    public string ISBN { get; set; }
    public string Author { get; set; }
    public string DeweyIndex { get; set; }
    public string Publisher { get; set; }
    public string Edition { get; set; }
    public int PublicationYear { get; set; }
    public string Description { get; set; }
    public decimal Cost { get; set; }
    public string ImageUrl { get; set; }
    public int Copies { get; set; }
    public int AvailableCopies { get; set; }
    public int DamagedCopies { get; set; }
    public int LostCopies { get; set; }
    public int UnknownStatusCopies { get; set; }
    public List<BookHeader> BookHeaders { get; set; }
}