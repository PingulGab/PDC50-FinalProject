using System;
using System.Text.Json;
using System.Text.Json.Serialization;

public class DateOnlyConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        try
        {
            string dateString = reader.GetString()?.Trim(); // Remove leading/trailing whitespace
            if (!string.IsNullOrEmpty(dateString))
            {
                // Parse the date strictly using the "yyyy-MM-dd" format
                return DateTime.ParseExact(dateString, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            }
            throw new JsonException("Invalid DateTime format.");
        }
        catch (Exception ex)
        {
            throw new JsonException($"Invalid DateTime format: {ex.Message}");
        }
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("yyyy-MM-dd")); // Save as yyyy-MM-dd
    }
}
