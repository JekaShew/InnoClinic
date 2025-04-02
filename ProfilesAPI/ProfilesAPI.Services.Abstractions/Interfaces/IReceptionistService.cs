using InnoClinic.CommonLibrary.Response;
using ProfilesAPI.Shared.DTOs.ReceptionistDTOs;

namespace ProfilesAPI.Services.Abstractions.Interfaces;

public interface IReceptionistService
{
    public Task<ResponseMessage<ReceptionistInfoDTO>> CreateReceptionistAsync(ReceptionistForCreateDTO receptionistForCreateDTO);
    public Task<ResponseMessage<ReceptionistInfoDTO>> UpdateReceptionistAsync(Guid receptionistId, ReceptionistForUpdateDTO receptionistForUpdateDTO);
    public Task<ResponseMessage> DeleteReceptionistByIdAsync(Guid receptionistId);
    public Task<ResponseMessage<ReceptionistInfoDTO>> GetReceptionistByIdAsync(Guid receptionistId);
    public Task<ResponseMessage<ICollection<ReceptionistTableInfoDTO>>> GetAllReceptionistsAsync(ReceptionistParameters? receptionistParameters);
}
