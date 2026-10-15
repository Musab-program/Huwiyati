namespace Huwiyati.API.Converters;

using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

/// <summary>
/// Custom JSON converter for serializing and deserializing <see cref="DateOnly"/> values across multiple date formats.
/// </summary>
public class DateOnlyJsonConverter : JsonConverter<DateOnly>
{
    private static readonly string[] Formats = new[]
    {
        "yyyy-MM-dd",
        "yyyy-M-d",
        "yyyy/MM/dd",
        "yyyy/M/d",
        "yyyy-MM-ddTHH:mm:ss",
        "yyyy-MM-ddTHH:mm:ss.FFFZ"
    };

    /// <summary>
    /// Reads and converts the JSON string to a <see cref="DateOnly"/> instance.
    /// </summary>
    /// <param name="reader">The reader to read from.</param>
    /// <param name="typeToConvert">The type to convert.</param>
    /// <param name="options">The serializer options.</param>
    /// <returns>The parsed <see cref="DateOnly"/> value.</returns>
    /// <exception cref="JsonException">Thrown when the string cannot be parsed into a valid date.</exception>
    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        if (string.IsNullOrWhiteSpace(value))
        {
            return default;
        }

        foreach (var format in Formats)
        {
            if (DateOnly.TryParseExact(value, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var result))
            {
                return result;
            }
        }

        if (DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTime))
        {
            return DateOnly.FromDateTime(dateTime);
        }

        throw new JsonException($"Unable to convert '{value}' to DateOnly.");
    }

    /// <summary>
    /// Writes the <see cref="DateOnly"/> value to JSON in ISO format (yyyy-MM-dd).
    /// </summary>
    /// <param name="writer">The writer to write to.</param>
    /// <param name="value">The value to serialize.</param>
    /// <param name="options">The serializer options.</param>
    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
    }
}

/// <summary>
/// Custom JSON converter for nullable <see cref="DateOnly"/> values.
/// </summary>
public class NullableDateOnlyJsonConverter : JsonConverter<DateOnly?>
{
    private readonly DateOnlyJsonConverter _converter = new();

    /// <summary>
    /// Reads and converts the JSON string or null literal to a nullable <see cref="DateOnly"/> instance.
    /// </summary>
    /// <param name="reader">The reader to read from.</param>
    /// <param name="typeToConvert">The type to convert.</param>
    /// <param name="options">The serializer options.</param>
    /// <returns>The parsed <see cref="DateOnly"/> value or null.</returns>
    public override DateOnly? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null) return null;
        return _converter.Read(ref reader, typeToConvert, options);
    }

    /// <summary>
    /// Writes the nullable <see cref="DateOnly"/> value to JSON.
    /// </summary>
    /// <param name="writer">The writer to write to.</param>
    /// <param name="value">The nullable value to serialize.</param>
    /// <param name="options">The serializer options.</param>
    public override void Write(Utf8JsonWriter writer, DateOnly? value, JsonSerializerOptions options)
    {
        if (!value.HasValue) writer.WriteNullValue();
        else _converter.Write(writer, value.Value, options);
    }
}
