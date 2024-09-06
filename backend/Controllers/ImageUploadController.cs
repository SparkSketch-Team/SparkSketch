using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
public class ImageUploadController : ApiController
{

    private readonly BlobServiceClient _blobServiceClient;
    private readonly string _containerName;
    private readonly ISketchRepository _sketchRepository;


    public ImageUploadController(IConfiguration configuration, ILogger<ImageUploadController> logger, ISketchRepository sketchRepository) : base(logger)
    {

        string storageConnectionString = configuration.GetConnectionString("AzureStorageConnectionString");
        _blobServiceClient = new BlobServiceClient(storageConnectionString);

        // You can set your container name here
        _containerName = "uploadedfiles";

        // Ensure the container exists
        var blobContainerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        blobContainerClient.CreateIfNotExists(PublicAccessType.Blob);

        _sketchRepository = sketchRepository;
    }

    [HttpPost("upload")]
    //[Authorize]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {

        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return FailMessage("Error: No User Logged In");
            _logger.LogInformation("Upload File, no user detected");
        }


        _logger.LogInformation("Image Upload Called");

        if (file == null || file.Length == 0)
        {
            _logger.LogError("Image Upload Failed");
            return FailMessage("No file uploaded.");
        }

        var currentUserId = User.Claims.FirstOrDefault(c => c.Type == SparkSketchClaims.UserId)?.Value;
        if (string.IsNullOrEmpty(currentUserId))
        {
            _logger.LogError("User not found");
            return FailMessage("User not found.");
        }

        var blobContainerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        var blobClient = blobContainerClient.GetBlobClient(file.FileName);

        using (var stream = file.OpenReadStream())
        {
            await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = file.ContentType });
        }

        // Create a new Sketch
        var sketch = new Sketch
        {
            ArtistID = Guid.Parse(currentUserId),
            MediaUrl = blobClient.Uri.ToString(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Save Sketch using the repository
        var createdSketch = await _sketchRepository.CreateSketchAsync(sketch);

        _logger.LogInformation("Image upload and sketch creation successful");

        var response = new { filePath = blobClient.Uri.ToString(), sketchId = createdSketch.PostId };
        _logger.LogInformation($"Response: {JsonConvert.SerializeObject(response)}");

        return SuccessMessage(response);
    }

    [HttpDelete("delete/{postId}")]
    public async Task<IActionResult> DeleteSketch(int postId)
    {
        _logger.LogInformation($"Deleting sketch with PostId: {postId}");

        var isDeleted = await _sketchRepository.DeleteSketchAsync(postId);

        if (!isDeleted)
        {
            _logger.LogError($"Sketch with PostId {postId} not found");
            return NotFound($"Sketch with PostId {postId} not found");
        }

        _logger.LogInformation($"Sketch with PostId {postId} deleted successfully");
        return SuccessMessage($"Sketch with PostId {postId} deleted successfully");
    }


}