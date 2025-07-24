﻿namespace Sistema_Produccion_3_Backend.Services
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    public class JsonDateConverter : JsonConverter<DateTime?>
    {
        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String && DateTime.TryParse(reader.GetString(), out DateTime date))
            {
                return date;
            }
            return null;
        }

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
            {
                writer.WriteStringValue(value.Value.ToString("yyyy-MM-dd"));
            }
            else
            {
                writer.WriteNullValue();
            }
        }
    }
}
