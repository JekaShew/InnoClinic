using InnoClinic.CommonLibrary.Response;

namespace ProfilesAPI.Services.Abstractions.Interfaces;

public interface IOfficeService
{
    public Task<ResponseMessage> RequestCheckOfficeConsistancyAsync();
}
