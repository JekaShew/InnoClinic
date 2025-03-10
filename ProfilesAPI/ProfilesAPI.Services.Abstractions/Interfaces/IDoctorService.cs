using InnoClinic.CommonLibrary.Response;
using Microsoft.AspNetCore.Http;
using ProfilesAPI.Shared.DTOs.DoctorDTOs;

namespace ProfilesAPI.Services.Abstractions.Interfaces;

public interface IDoctorService
{
    public Task<ResponseMessage> AddDoctorAsync(DoctorForCreateDTO doctorForCreateDTO, IFormFile file);
    public Task<ResponseMessage> UpdateDoctorAsync(Guid doctorId, DoctorForUpdateDTO doctorForUpdateDTO, IFormFile? file);
    public Task<ResponseMessage> DeleteDoctorByIdAsync(Guid doctorId);
    public Task<ResponseMessage<DoctorInfoDTO>> GetDoctorByIdAsync(Guid doctorId);
    public Task<ResponseMessage<ICollection<DoctorTableInfoDTO>>> GetAllDoctorsAsync();
}
