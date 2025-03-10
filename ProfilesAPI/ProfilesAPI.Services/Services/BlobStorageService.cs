using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;
using ProfilesAPI.Services.Abstractions.Interfaces;
using ProfilesAPI.Services.Extensions;
using ProfilesAPI.Shared.DTOs;

namespace ProfilesAPI.Services.Services;

public class BlobStorageService() : IBlobStorageService
{
    private readonly BlobContainerTitles _blobContainerTitles;
    private readonly BlobServiceClient _blobServiceClient;

    public BlobStorageService(
            BlobServiceClient blobServiceClient, 
            IOptions<BlobContainerTitles> options)
       : this(options)
    {
        _blobServiceClient = blobServiceClient;
    }

    public BlobStorageService(IOptions<BlobContainerTitles> options)
        :this()
    {
        _blobContainerTitles = options.Value;
    }

    //public BlobStorageService(IOptions<BlobContainerTitles> options)
    //{
    //    _blobContainerTitles = options.Value;    
    //}
    public async Task DeleteAsync(Guid fileId)
    {
        BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(_blobContainerTitles.ContainerTitle);
        BlobClient blobClient = blobContainerClient.GetBlobClient(fileId.ToString());
        await blobClient.DeleteIfExistsAsync();         
    }

    public async Task<FileResponse> DownloadAsync(Guid fileId)
    {
        BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(_blobContainerTitles.ContainerTitle);
        BlobClient blobClient = blobContainerClient.GetBlobClient(fileId.ToString());
        Response<BlobDownloadResult> response = await blobClient.DownloadContentAsync();

        return new FileResponse(response.Value.Content.ToStream(), response.Value.Details.ContentType);
    }

    public async Task<Guid> UploadAsync(Stream stream, string contentType)
    {
        BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(_blobContainerTitles.ContainerTitle);
        var fileId = Guid.NewGuid();
        BlobClient blobClient = containerClient.GetBlobClient(fileId.ToString());
        await blobClient.UploadAsync(
                stream,
                new BlobHttpHeaders { ContentType = contentType });
        
        return fileId;
    }
}
