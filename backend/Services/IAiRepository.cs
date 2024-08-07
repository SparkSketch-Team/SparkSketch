using OpenAI_API.Chat;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IAiRepository
{
    Task<ChatResult> CreateNewPrompt();
    Task<Prompt> GetPrompt();
    Task<List<string>> GetAllPromptTextsAsync();
}

