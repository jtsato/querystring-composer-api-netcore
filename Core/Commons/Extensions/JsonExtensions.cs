using System;
using System.Buffers;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace Core.Commons.Extensions;

public static class JsonExtensions
{
    public static string ToJsonString(this JsonDocument jsonDocument, bool indented = false)
    {
        return ToJsonString(jsonDocument, new JsonWriterOptions {Indented = indented});
    }

    private static string ToJsonString(this JsonDocument jsonDocument, JsonWriterOptions jsonWriterOptions)
    {
        using MemoryStream stream = new MemoryStream();
        Utf8JsonWriter writer = new Utf8JsonWriter(stream, jsonWriterOptions);
        jsonDocument.WriteTo(writer);
        writer.Flush();
        return Encoding.UTF8.GetString(stream.ToArray());
    }

    public static T ToObject<T>(this JsonElement jsonElement, JsonSerializerOptions options = null)
    {
        ArrayBufferWriter<byte> bufferWriter = new ArrayBufferWriter<byte>();

        using (Utf8JsonWriter writer = new Utf8JsonWriter(bufferWriter))
        {
            jsonElement.WriteTo(writer);
        }

        return JsonSerializer.Deserialize<T>(bufferWriter.WrittenSpan, options);
    }

    public static JsonElement SelectToken(this JsonElement jsonElement, string path)
    {
        while (true)
        {
            if (string.IsNullOrEmpty(path)) return jsonElement;
            string propertyName = path.SubstringAfter(".").SubstringBefore(".");
            string indexAsString = propertyName.SubstringAfter("[").SubstringBefore("]");
            if (string.IsNullOrEmpty(indexAsString))
            {
                jsonElement = jsonElement.GetProperty(propertyName);
            }
            else
            {
                int searchedIndex = Convert.ToInt32(indexAsString);
                jsonElement = GetPropertyAtIndex(jsonElement, propertyName, searchedIndex);
            }

            path = path.SubstringAfter(propertyName);
        }
    }

    private static JsonElement GetPropertyAtIndex(JsonElement jsonElement, string propertyName, int searchedIndex)
    {
        string arrayPropertyName = propertyName.SubstringBefore("[");
        JsonElement.ArrayEnumerator arrayEnumerator = jsonElement.GetProperty(arrayPropertyName).EnumerateArray();
        int maxIndex = arrayEnumerator.Count() - 1;
        ValidateSearchedIndex(searchedIndex, maxIndex, arrayPropertyName);
        return arrayEnumerator.ElementAt(searchedIndex);
    }

    private static void ValidateSearchedIndex(int searchedIndex, int maxIndex, string arrayPropertyName)
    {
        if (searchedIndex < 0)
        {
            throw new ArgumentException($"The index {searchedIndex} is invalid.");
        }

        if (searchedIndex > maxIndex)
        {
            throw new ArgumentException($"The index {searchedIndex} is out of range of '{arrayPropertyName}' array. The max index is {maxIndex}.");
        }
    }
}