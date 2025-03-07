using ProfilesAPI.Shared.DTOs;

namespace ProfilesAPI.Services.Abstractions.Interfaces;

public interface IBlobStorageService
{
    public Task<Guid> UploadAsync(Stream stream, string ContentType);
    public Task<FileResponse> DownloadAsync(Guid blobId);
    public Task DeleteAsync(Guid blobId);


}


