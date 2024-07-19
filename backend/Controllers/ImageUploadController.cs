using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ImageUploadController : ApiController
{

    private readonly string _targetFilePath; // need to transfer to repository class

    public ImageUploadController(IConfiguration configuration)
    {
        //aRepo = new AiRepository(configuration);
        _targetFilePath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");
        if (!Directory.Exists(_targetFilePath))
        {
            Directory.CreateDirectory(_targetFilePath);
        }
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        var filePath = Path.Combine(_targetFilePath, file.FileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return Ok(new { filePath });
    }

}