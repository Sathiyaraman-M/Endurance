using Quark.Core.Domain.Common;

namespace Quark.Core.Domain.Entities;

public class Book : AuditableEntity<int>
{
    public string Name { get; set; }
    public string ISBN { get; set; }
    public string Author { get; set; }
    public string DeweyIndex { get; set; }
    public string Publisher { get; set; }
    public string Edition { get; set; }
    public int PublicationYear { get; set; }
    public string Description { get; set; }
    public string Barcode { get; set; }
    public decimal Cost { get; set; }
    public string ImageUrl { get; set; }
    public bool IsAvailable { get; set; }
    public string Condition { get; set; }
}