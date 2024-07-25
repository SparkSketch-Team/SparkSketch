using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ImageUploadController : ApiController
{

    private readonly BlobServiceClient _blobServiceClient;
    private readonly string _containerName;

    public ImageUploadController(IConfiguration configuration)
    {
        string accountName = configuration["AzureStorage:AccountName"];
        string accountKey = configuration["AzureStorage:AccountKey"];
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
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        var blobContainerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        var blobClient = blobContainerClient.GetBlobClient(file.FileName);

        using (var stream = file.OpenReadStream())
        {
            await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = file.ContentType });
        }

        return Ok(new { filePath = blobClient.Uri.ToString() });
    }

    [HttpGet("download/{fileName}")]
    public async Task<IActionResult> DownloadFile(string fileName)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            return BadRequest("File name cannot be null or empty.");
        }

        var blobContainerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        var blobClient = blobContainerClient.GetBlobClient(fileName);

        if (!await blobClient.ExistsAsync())
        {
            return NotFound("File not found.");
        }

        var blobDownloadInfo = await blobClient.DownloadAsync();

        return File(blobDownloadInfo.Value.Content, blobDownloadInfo.Value.ContentType, fileName);
    }

    [HttpGet("list")]
    public async Task<IActionResult> ListFiles()
    {
        var blobContainerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        var files = new List<string>();
        var uriBuilder = new UriBuilder(_blobServiceClient.Uri);

        await foreach (BlobItem blobItem in blobContainerClient.GetBlobsAsync())
        {
            uriBuilder.Path = $"{_containerName}/{blobItem.Name}";
            files.Add(uriBuilder.ToString());
        }

        return Ok(files);
    }


}