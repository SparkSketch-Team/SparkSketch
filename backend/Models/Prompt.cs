using System;
using System.Text.Json;
using System.Text.Json.Serialization;

public class Prompt
{
    [JsonPropertyName("theme")]
    public string Theme { get; set; }

    [JsonPropertyName("prompt")]
    public string Description { get; set; }
}

public class JsonResponse
{
    [JsonPropertyName("prompt")]
    public Prompt Prompt { get; set; }
}
