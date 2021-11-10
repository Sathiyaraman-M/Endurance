using Quark.Core.Domain.Common;

namespace Quark.Core.Domain.Entities;

public class Checkout : AuditableEntity<int>
{
    public virtual Book Book { get; set; }

    public int BookId { get; set; }

    public virtual Patron Patron { get; set; }

    public int PatronId { get; set; }

    public DateTime CheckedOutSince { get; set; }

    public DateTime ExpectedCheckInDate { get; set; }

    public DateTime? CheckedOutUntil { get; set; }
}