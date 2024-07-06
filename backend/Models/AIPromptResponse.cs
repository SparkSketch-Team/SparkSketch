using System;
using System.Text.Json;
using System.Text.Json.Serialization;

public class AIPromptResponse
{
    [JsonPropertyName("theme")]
    public string Theme { get; set; }

    [JsonPropertyName("prompt")]
    public string Description { get; set; }
}

public class JsonResponse
{
    [JsonPropertyName("prompt")]
    public AIPromptResponse Prompt { get; set; }
}
