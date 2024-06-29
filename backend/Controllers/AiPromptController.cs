using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

[ApiController]
[Route("api/[controller]")]
public class AiController : ApiController
{

    private AiRepository aRepo = new AiRepository();

    [HttpPost]
    [Route("GetResponse")]
    public async Task<JsonResult> GetResponse([FromBody] JsonElement aiFunctionInfo)
    {
        using (aRepo)
        {
            try
            {
                var response = await aRepo.getResponse(aiFunctionInfo);
                if (response == null)
                {
                    return FailMessage();
                }
                return SuccessMessage("Successfully connected to the AI");
            }
            catch (Exception e)
            {
                return FailMessage(e.Message);
            }
        }
    }

}
