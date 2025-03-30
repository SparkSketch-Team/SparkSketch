using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Microsoft.AspNetCore.Cors;

[ApiController]
[Route("api/[controller]")]
public class AiController : ApiController
{
    private readonly IAiRepository _aiRepository;

    public AiController(IConfiguration configuration, IAiRepository aiRepository, ILogger<AiController> logger) : base(logger)
    {
        _aiRepository = aiRepository;
    }

    [HttpPost]
    [Route("CreateNewPrompt")]
    public async Task<JsonResult> CreateNewPrompt()
    {
            try
            {
                _logger.LogInformation("CreateNewPrompt called.");
                var response = await _aiRepository.CreateNewPrompt();
                if (response == null)
                {
                    _logger.LogWarning("CreateNewPrompt returned null.");
                    return FailMessage();
                }
                _logger.LogInformation("CreateNewPrompt successful: {Response}", response);
                return SuccessMessage(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception in CreateNewPrompt.");
                return FailMessage(e.Message);
            }
    }

    [HttpGet]
    [Route("GetPrompt")]
    public async Task<JsonResult> GetPrompt()
    {

            try
            {
                _logger.LogInformation("GetPrompt called.");
                var response = await _aiRepository.GetPrompt();
                if (response == null)
                {
                    _logger.LogWarning("GetPrompt returned null.");
                    return FailMessage("No prompt available.");
                }
                _logger.LogInformation("GetPrompt successful: {Response}", response);
                return SuccessMessage(response);
            } catch (Exception e)
            {
                _logger.LogError(e, "Exception in GetPrompt.");
                return FailMessage(e.Message);
            }
    }

    [HttpPost]
    [Route("UpdatePromptRating")]
    public async Task<JsonResult> UpdatePromptRating([FromBody] RatingInfo ratingInfo) // Ensure [FromBody] is present
    {
        if (ratingInfo == null)
        {
            _logger.LogWarning("RatingInfo is null in request.");
            return FailMessage("Request body is missing or incorrect.");
        }

        try
        {
            _logger.LogInformation("RatingPrompt called with RatingInfo: {@RatingInfo}", ratingInfo);

            var response = await _aiRepository.UpdatePromptRating(ratingInfo);
            if (response == null)
            {
                _logger.LogWarning("RatingPrompt returned null: Invalid Prompt ID.");
                return FailMessage("Invalid Prompt ID");
            }

            return SuccessMessage(new
            {
                Id = response.Id,
                PromptId = response.PromptId,
                RatingValue = response.RatingValue.ToString(), // Ensure enum is returned as a string
                Comment = response.Comment,
                CreatedAt = response.CreatedAt,
                UpdatedAt = response.UpdatedAt
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Exception in RatingPrompt.");
            return FailMessage(e.Message);
        }
    }



}
