using InnoClinic.CommonLibrary.Response;

namespace ProfilesAPI.Services.Abstractions.Interfaces;

public interface ISpecializationService
{
    public Task<ResponseMessage> RequestCheckSpecializationConsistancyAsync();
}
