using Quark.Core.Domain.Common;

namespace Quark.Core.Domain.Entities;

public class Patron : AuditableEntity<int>
{
    public string RegisterId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Email { get; set; }
    public string Mobile { get; set; }
    public decimal CurrentFees { get; set; }
    public DateTime Issued { get; set; }
    public virtual IEnumerable<Checkout> Checkouts { get; set; }
    public int MultipleCheckoutLimit { get; set; }
}