using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Text.Json.Nodes;
using System.Text.Json;
using OpenAI_API.Chat;
using OpenAI_API.Models;

// In SDK-style projects such as this one, several assembly attributes that were historically
// defined in this file are now automatically added during build and populated with
// values defined in project properties. For details of which attributes are included
// and how to customise this process see: https://aka.ms/assembly-info-properties


// Setting ComVisible to false makes the types in this assembly not visible to COM
// components.  If you need to access a type in this assembly from COM, set the ComVisible
// attribute to true on that type.

[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM.

[assembly: Guid("f8e357f4-bde9-4bca-b212-2c161df506ec")]


public class AiRepository : BaseRepository
{
    private List<Prompt> _prompts = new();

    public async Task<ChatResult> CreateNewPrompt()
    {
        var configBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.development.json");

        IConfiguration configuration = configBuilder.Build();
        string apiKey = configuration["OpenAI:ApiKey"];

        var api = new OpenAI_API.OpenAIAPI(apiKey);

        ChatRequest chatRequest = new ChatRequest()
        {
            Model = Model.ChatGPTTurbo,
            Temperature = 0.0,
            MaxTokens = 500,
            ResponseFormat = ChatRequest.ResponseFormats.JsonObject,
            Messages = new ChatMessage[] {
        new ChatMessage(ChatMessageRole.System, "You are a helpful assistant designed to output drawing prompts in JSON format."),
        new ChatMessage(ChatMessageRole.System, "These are the current prompts: " + _prompts.ToString()),
        new ChatMessage(ChatMessageRole.User, "Give a new drawing prompt for a quick doodle.  Return JSON of a 'prompt' dictionary with the theme and prompt as the string value.")
    }
        };

        var results = await api.Chat.CreateChatCompletionAsync(chatRequest);
        Console.WriteLine(results);

        JsonResponse? jsonResponse = JsonSerializer.Deserialize<JsonResponse>(results.ToString());

        if (jsonResponse != null)
        {
            _prompts.Add(jsonResponse.Prompt);
        }


        return results;
    }

    public async Task<Prompt> GetPrompt()
    {
        if (_prompts.Count > 0)
        {
            return _prompts[0];
        }
        else
        {
            await CreateNewPrompt();
            return _prompts[0];
        }
    }

}
