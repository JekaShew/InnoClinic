using InnoClinic.CommonLibrary.Response;
using ProfilesAPI.Shared.DTOs.SpecializationDTOs;

namespace ProfilesAPI.Services.Abstractions.Interfaces;

public interface ISpecializationService
{
    public Task<ResponseMessage<Guid>> CreateSpecializationAsync(SpecializationForCreateDTO specializationForCreateDTO);
    public Task<ResponseMessage<SpecializationInfoDTO>> UpdateSpecializationAsync(Guid specializationId, SpecializationForUpdateDTO specializationForUpdateDTO);
    public Task<ResponseMessage> DeleteSpecializationByIdAsync(Guid specializationId);
    public Task<ResponseMessage<SpecializationInfoDTO>> GetSpecializationByIdAsync(Guid specializationId);
    public Task<ResponseMessage<ICollection<SpecializationTableInfoDTO>>> GetAllSpecializationsAsync();
}
