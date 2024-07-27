using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Microsoft.AspNetCore.Cors;

[ApiController]
[Route("api/[controller]")]
public class AiController : ApiController
{

    private AiRepository aRepo;

    public AiController(IConfiguration configuration, ILogger<AiController> logger) : base(logger)
    {
        aRepo = new AiRepository(configuration);
    }

    [HttpPost]
    [Route("CreateNewPrompt")]
    public async Task<JsonResult> CreateNewPrompt()
    {
        using (aRepo)
        {
            try
            {
                _logger.LogInformation("CreateNewPrompt called.");
                var response = await aRepo.CreateNewPrompt();
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
    }

    [HttpGet]
    [Route("GetPrompt")]
    public async Task<JsonResult> GetPrompt()
    {
        using (aRepo)
        {
            try
            {
                _logger.LogInformation("GetPrompt called.");
                var response = await aRepo.GetPrompt();
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
    }

}
