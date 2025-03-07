using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using ProfilesAPI.Services.Abstractions.Interfaces;
using ProfilesAPI.Shared.DTOs;

namespace ProfilesAPI.Services.Services;

public class BlobStorageService(BlobServiceClient blobServiceClient) : IBlobStorageService
{
    private readonly string _containerTitle;
    public BlobStorageService()
    {
        _containerTitle = IConfiguration.GetSection("BlobStorage:ContainerTitle").Value;    
    }
    public async Task DeleteAsync(Guid fileId)
    {
        BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(_containerTitle);
        BlobClient blobClient = blobContainerClient.GetBlobClient(fileId.ToString());
        await blobClient.DeleteIfExistsAsync();         
    }

    public async Task<FileResponse> DownloadAsync(Guid fileId)
    {
        BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(_containerTitle);
        BlobClient blobClient = blobContainerClient.GetBlobClient(fileId.ToString());
        Response<BlobDownloadResult> response = await blobClient.DownloadContentAsync();

        return new FileResponse(response.Value.Content.ToStream(), response.Value.Details.ContentType);
    }

    public async Task<Guid> UploadAsync(Stream stream, string contentType)
    {
        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(_containerTitle);
        var fileId = Guid.NewGuid();
        BlobClient blobClient = containerClient.GetBlobClient(fileId.ToString());
        await blobClient.UploadAsync(
                stream,
                new BlobHttpHeaders { ContentType = contentType });
        
        return fileId;
    }
}
