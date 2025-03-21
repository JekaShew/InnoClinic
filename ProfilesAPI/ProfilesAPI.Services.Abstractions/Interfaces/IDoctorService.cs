using InnoClinic.CommonLibrary.Response;
using ProfilesAPI.Shared.DTOs.DoctorDTOs;

namespace ProfilesAPI.Services.Abstractions.Interfaces;

public interface IDoctorService
{
    public Task<ResponseMessage<Guid>> CreateDoctorAsync(DoctorForCreateDTO doctorForCreateDTO);
    public Task<ResponseMessage<DoctorInfoDTO>> UpdateDoctorAsync(Guid doctorId, DoctorForUpdateDTO doctorForUpdateDTO);
    public Task<ResponseMessage> UpdateDoctorSpecializationsAsync(Guid doctorId, IEnumerable<DoctorSpecializationForUpdateDTO> doctorSpecializationForUpdateDTOs);
    public Task<ResponseMessage> DeleteDoctorByIdAsync(Guid doctorId);
    public Task<ResponseMessage<DoctorInfoDTO>> GetDoctorByIdAsync(Guid doctorId);
    public Task<ResponseMessage<ICollection<DoctorTableInfoDTO>>> GetAllDoctorsAsync(DoctorParameters? doctorParameters);
}
