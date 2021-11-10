using System.Text.Json;
using Quark.Core.Interfaces.Serialization.Serializers;
using Microsoft.Extensions.Options;
using Quark.Core.Serialization.Options;

namespace Quark.Core.Serialization.Serializers;

public class SystemTextJsonSerializer : IJsonSerializer
{
    private readonly JsonSerializerOptions _options;

    public SystemTextJsonSerializer(IOptions<SystemTextJsonOptions> options)
    {
        _options = options.Value.JsonSerializerOptions;
    }

    public T Deserialize<T>(string data)
        => JsonSerializer.Deserialize<T>(data, _options);

    public string Serialize<T>(T data)
        => JsonSerializer.Serialize(data, _options);
}