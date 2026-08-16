using System.Text.Json.Serialization;

namespace SiloAI.Application.Shared.Features;

public class ApiRequest
{
    /// <summary>
    /// Should be: RestAPI
    /// </summary>
    [JsonPropertyName("interface")]
    public string Interface { get; set; }

    /// <summary>
    /// Name of your method in current project
    /// </summary>
    [JsonPropertyName("method")]
    public string Method { get; set; }

    [JsonPropertyName("parameters")]
    public object Parameters { get; set; }
}

public class ApiResponse<T>
{
    [JsonPropertyName("successful")]
    public bool Successful { get; set; }

    [JsonPropertyName("value")]
    public T Value { get; set; }

    [JsonPropertyName("messages")]
    public string[]? Messages { get; set; }
}

public class ApiResponse
{
    public bool Successful { get; set; }
    public object Value { get; set; }
    public string[] Messages { get; set; }
}
