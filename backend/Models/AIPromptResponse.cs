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

public class AiPromptJsonResponse
{
    [JsonPropertyName("prompt")]
    public AIPromptResponse aIPromptResponse { get; set; }

    public static Prompt ConvertAiResponeToPrompt(AIPromptResponse aiPromptResponse)
    {
        return new Prompt
        {
            Theme = aiPromptResponse.Theme,
            PromptText = aiPromptResponse.Description,
            IsActive = true, // Assuming new prompts are active by default
            PromptDate = DateTime.Now,
            CreateDate = DateTime.Now,
            //ModifiedDate = DateTime.Now,
            CreatedBy = "system", // Assuming system created this entry
            //ModifiedBy = "system" // Assuming system modified this entry
        };
    }
}
