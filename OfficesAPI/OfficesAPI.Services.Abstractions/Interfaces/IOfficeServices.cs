using InnoClinic.CommonLibrary.Response;
using OfficesAPI.Shared.DTOs;

namespace OfficesAPI.Services.Abstractions.Interfaces
{
    public interface IOfficeServices
    {
        public Task<List<OfficeTableInformationDTO>> TakeAllOffices();
        public Task<OfficeInformationDTO> TakeOfficeById(string officeId);
        public Task<CustomResponse> AddOffice(OfficeDTO officeDTO);
        public Task<CustomResponse> UpdateOffice(OfficeDTO officeDTO);
        public Task<CustomResponse> DeleteOfficeById(string officeId);
        public Task<CustomResponse> ChangeStatusOfOfficeById(string officeId);

        // Think about Implementation
        //public Task<CustomResponse> AddPhotoOfOfficeById(string officeId);

        // Think about Implementation
        //public Task<CustomResponse> DeletePhotoOfOfficeById(string officeId);
    }
}
