using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NoteBin.Models.Json
{
    public class DateTimeToUtcMillisecondsConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return TimeUtils.FromUnixTimeMilliseconds(reader.GetInt64());
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(TimeUtils.ToUnixTimeMilliseconds(value));
        }
    }
}