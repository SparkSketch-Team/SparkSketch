using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Text.Json.Nodes;
using System.Text.Json;
using OpenAI_API.Chat;
using OpenAI_API.Models;
using System;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Transactions;
using Microsoft.AspNetCore.Components;

public class AiRepository : BaseRepository, IAiRepository
{

    private string apiKey;
    public AiRepository(IConfiguration config) : base(config)
    {
        apiKey = _config["OpenAI:ApiKey"];
    }

    public async Task<ChatResult> CreateNewPrompt()
    {

        var api = new OpenAI_API.OpenAIAPI(apiKey);

        List<string> oldPrompts = await GetAllPromptTextsAsync();
        StringBuilder sb = new StringBuilder();

        foreach (var prompt in oldPrompts)
        {
            if (sb.Length > 0)
            {
                sb.Append(" "); // Add separator if needed
            }
            sb.Append(prompt);
        }

        string combinedString = sb.ToString();

        ChatRequest chatRequest = new ChatRequest()
        {
            Model = Model.ChatGPTTurbo,
            Temperature = 0.0,
            MaxTokens = 500,
            ResponseFormat = ChatRequest.ResponseFormats.JsonObject,
            Messages = new ChatMessage[] {
        new ChatMessage(ChatMessageRole.System, "You are a helpful assistant designed to output drawing prompts in JSON format."),
        //TODO: fix, this is really stupid
        new ChatMessage(ChatMessageRole.System, "The format is \"prompt\": { \"theme\": \"\", \"prompt\" }. Do not deviate from this format."),
        new ChatMessage(ChatMessageRole.System, "These are the old prompts: " + combinedString + " The prompt you return must be different that the other prompts."),
        new ChatMessage(ChatMessageRole.User, "Give a new drawing prompt for a quick doodle.  Return JSON of a 'prompt' dictionary with the 'theme' and 'prompt' as the string value.")
    }
        };

        var results = await api.Chat.CreateChatCompletionAsync(chatRequest);
        Console.WriteLine(results);

        AiPromptJsonResponse? jsonResponse = JsonSerializer.Deserialize<AiPromptJsonResponse>(results.ToString());

        if (jsonResponse != null)
        {
            Prompt newPrompt = AiPromptJsonResponse.ConvertAiResponeToPrompt(jsonResponse.aIPromptResponse);

            // Save to database
            try
            {
                db.Prompts.Add(newPrompt);
                await db.SaveChangesAsync();
            } catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
                if (e.InnerException != null)
                {
                    Console.WriteLine("Inner exception: " + e.InnerException.Message);
                }
            }
        }


        return results;
    }

    public async Task<Prompt> GetPrompt()
    {
        // Get the current date
        DateTime currentDate = DateTime.Today;

        // Find the first prompt with the current date
        var prompt = db.Prompts.FirstOrDefault(p => p.PromptDate.Date == currentDate);

        if (prompt != null)
        {
            return prompt;
        }
        else
        {
            await CreateNewPrompt();
            // Assuming CreateNewPrompt adds a new prompt to the _prompts list
            return db.Prompts.First(p => p.PromptDate.Date == currentDate);
        }
    }

    public async Task<List<string>> GetAllPromptTextsAsync()
    {
    return await db.Prompts.Select(p => p.PromptText).ToListAsync();
    }

    public async Task<Rating> UpdatePromptRating(RatingInfo ratingInfo)
    {
        var prompt = await db.Prompts.FindAsync(ratingInfo.PromptId);
        if (prompt == null)
        {
            return null; // Prompt doesn't exist
        }

        var existingRating = await db.Ratings.FirstOrDefaultAsync(r => r.Id == ratingInfo.Id);

        if (existingRating == null)
        {
            // Create a new rating
            var newRating = new Rating
            {
                PromptId = ratingInfo.PromptId,
                RatingValue = ratingInfo.RatingValue,
                Comment = ratingInfo.Comment,
                CreatedAt = DateTime.UtcNow
            };

            db.Ratings.Add(newRating);
            await db.SaveChangesAsync();
            return newRating; // Return the newly created rating with its ID
        }
        else
        {
            // Update the existing rating
            existingRating.RatingValue = ratingInfo.RatingValue;
            existingRating.Comment = ratingInfo.Comment;
            existingRating.UpdatedAt = DateTime.UtcNow;

            await db.SaveChangesAsync();
            return existingRating;
        }
    }



}
