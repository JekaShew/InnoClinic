using ProfilesAPI.Shared.DTOs.ReceptionistDTOs;

namespace ProfilesAPI.Services.Abstractions.Interfaces;

public interface IReceptionistService
{
    public Task AddReceptionistAsync(ReceptionistForCreateDTO receptionistForCreateDTO);
    public Task UpdateReceptionistAsync(Guid receptionistId, ReceptionistForUpdateDTO receptionistForUpdateDTO);
    public Task DeleteReceptionistByIdAsync(Guid receptionistId);
    public Task<ReceptionistInfoDTO> GetReceptionistByIdAsync(Guid receptionistId);
    public Task<ICollection<ReceptionistTableInfoDTO>> GetAllReceptionistsAsync();
}
