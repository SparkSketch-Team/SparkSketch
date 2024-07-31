using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

[ApiController]
[Route("api/[controller]")]
public class ImageUploadController : ApiController
{

    private readonly BlobServiceClient _blobServiceClient;
    private readonly string _containerName;


    public ImageUploadController(IConfiguration configuration, ILogger<ImageUploadController> logger) : base(logger)
    {

        string storageConnectionString = configuration.GetConnectionString("AzureStorageConnectionString");
        _blobServiceClient = new BlobServiceClient(storageConnectionString);

        // You can set your container name here
        _containerName = "uploadedfiles";

        // Ensure the container exists
        var blobContainerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        blobContainerClient.CreateIfNotExists(PublicAccessType.Blob);
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {

        _logger.LogInformation("Image Upload Called");
        if (file == null || file.Length == 0)
        {
            _logger.LogError("Image Upload Failed");
            return FailMessage("No file uploaded.");
        }

        var blobContainerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        var blobClient = blobContainerClient.GetBlobClient(file.FileName);

        using (var stream = file.OpenReadStream())
        {
            await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = file.ContentType });
        }

        _logger.LogInformation("Image upload successful");
        var response = new { filePath = blobClient.Uri.ToString() };
        _logger.LogInformation($"Response: {JsonConvert.SerializeObject(response)}");
        return SuccessMessage(response);
    }

    [HttpGet("download/{fileName}")]
    public async Task<IActionResult> DownloadFile(string fileName)
    {
        _logger.LogInformation("Download File Called");
        if (string.IsNullOrEmpty(fileName))
        {
            _logger.LogError("Download File Failed");
            return FailMessage("File name cannot be null or empty.");
        }

        var blobContainerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        var blobClient = blobContainerClient.GetBlobClient(fileName);

        if (!await blobClient.ExistsAsync())
        {
            _logger.LogError("Download File Failed: Image not Found");
            return NotFound("File not found.");
        }

        var blobDownloadInfo = await blobClient.DownloadAsync();

        _logger.LogInformation("Image download successful");

        return File(blobDownloadInfo.Value.Content, blobDownloadInfo.Value.ContentType, fileName);
    }

    [HttpGet("list")]
    public async Task<IActionResult> ListFiles()
    {

        _logger.LogInformation("Download File Called");
        var blobContainerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        var files = new List<string>();
        var uriBuilder = new UriBuilder(_blobServiceClient.Uri);

        await foreach (BlobItem blobItem in blobContainerClient.GetBlobsAsync())
        {
            uriBuilder.Path = $"{_containerName}/{blobItem.Name}";
            files.Add(uriBuilder.ToString());
        }

        _logger.LogInformation("Files sent to user");

        return SuccessMessage(files);
    }


}