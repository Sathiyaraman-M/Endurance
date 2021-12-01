﻿namespace Quark.Core.Domain.Entities;

public class Checkout : AuditableEntity<int>
{
    public virtual BookHeader BookHeader { get; set; }

    public Guid BookHeaderId { get; set; }

    public virtual Patron Patron { get; set; }

    public int PatronId { get; set; }

    public DateTime CheckedOutSince { get; set; }

    public DateTime ExpectedCheckInDate { get; set; }

    public DateTime? CheckedOutUntil { get; set; }
}