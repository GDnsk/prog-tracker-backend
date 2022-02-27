using System.Text.Json;
using System.Text.Json.Serialization;
using ProgTracker.Domain.Model.Database;

namespace ProgTracker.WebApi.Converters;

public class IdConverter : JsonConverter<Id>
{
    public override Id Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return new Id(value);
    }

    public override void Write(Utf8JsonWriter writer, Id value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Value);
    }
}