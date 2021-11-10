using Quark.Core.Domain.Common;

namespace Quark.Core.Domain.Entities;

public class AssetAvailability : IEntity<int>
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }
}