using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

[ApiController]
[Route("api/[controller]")]
public class AiController : ApiController
{

    private AiRepository aRepo = new AiRepository();

    [HttpPost]
    [Route("CreateNewPrompt")]
    public async Task<JsonResult> CreateNewPrompt()
    {
        using (aRepo)
        {
            try
            {
                var response = await aRepo.CreateNewPrompt();
                if (response == null)
                {
                    return FailMessage();
                }
                return SuccessMessage(response);
            }
            catch (Exception e)
            {
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
                var response = await aRepo.GetPrompt();
                if (response == null)
                {
                    return FailMessage("No prompt available.");
                }
                return SuccessMessage(response);
            } catch (Exception e)
            {
                return FailMessage(e.Message);
            }
        }
    }

}
