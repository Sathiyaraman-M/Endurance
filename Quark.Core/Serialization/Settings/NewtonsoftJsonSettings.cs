using Quark.Core.Interfaces.Serialization.Settings;
using Newtonsoft.Json;

namespace Quark.Core.Serialization.Settings;

public class NewtonsoftJsonSettings : IJsonSerializerSettings
{
    public JsonSerializerSettings JsonSerializerSettings { get; } = new();
}