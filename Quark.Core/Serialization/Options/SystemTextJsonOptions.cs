using System.Text.Json;
using Quark.Core.Interfaces.Serialization.Options;

namespace Quark.Core.Serialization.Options;

public class SystemTextJsonOptions : IJsonSerializerOptions
{
    public JsonSerializerOptions JsonSerializerOptions { get; } = new();
}