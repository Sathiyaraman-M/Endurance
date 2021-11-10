using Newtonsoft.Json;

namespace Quark.Core.Interfaces.Serialization.Settings;

public interface IJsonSerializerSettings
{
    /// <summary>
    /// Settings for <see cref="Newtonsoft.Json"/>.
    /// </summary>
    public JsonSerializerSettings JsonSerializerSettings { get; }
}