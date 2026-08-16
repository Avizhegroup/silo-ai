using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using Newtonsoft.Json.Linq;

namespace SiloAI.Shared;
public static class JsonTools
{
    public static string ConvertJsonElementToEncodedString(JsonElement element)
    {
        JsonSerializerOptions options = new()
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            WriteIndented = false
        };

        return JsonSerializer.Serialize(element, options);
    }

    public static string ConvertJsonObjectToEncodedString(JsonObject jsonObject)
    {
        JsonSerializerOptions options = new()
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            WriteIndented = false
        };

        return JsonSerializer.Serialize(jsonObject, options);
    }

    public static string GetFormattedInARowTextFromJson(string json, string separator = " ، ")
    {
        if (json.HasNoValue())
        { 
            return string.Empty;
        }

        try
        {
            var dict = JsonSerializer.Deserialize<Dictionary<string, object>>(json);

            if (dict is null)
            {
                return string.Empty;
            }

            var parts = dict
                .Select(kv => $"{kv.Key} : {kv.Value}");

            return string.Join(separator, parts);
        }
        catch (Exception ex)
        {
            return string.Empty;
        }
    }

    public static Dictionary<string, string?> ParseToDict(string? json)
    {
        var dict = new Dictionary<string, string?>();

        if (json.HasNoValue() || json.Trim() == "null")
        {
            return dict;
        }

        json = json.Trim();

        try
        {
            var obj = JObject.Parse(json);

            foreach (var p in obj.Properties())
            {
                dict[p.Name] = p.Value.Type == JTokenType.Null ? null : p.Value.ToString();
            }
        }
        catch (Exception ex)
        {
            return null;
        }

        return dict;
    }
}

