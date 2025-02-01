using Azure;
using InnoClinic.CommonLibrary.Response;
using OfficesAPI.Domain.IRepositories;
using OfficesAPI.Services.Abstractions.Interfaces;
using OfficesAPI.Shared.DTOs;
using OfficesAPI.Shared.Mappers;

namespace OfficesAPI.Services.Services
{
    public class OfficeServices : IOfficeServices
    {
        private readonly IOffice _officeRepository;
        public OfficeServices(IOffice officeRepository)
        {
            _officeRepository = officeRepository;
        }
        public async Task<CustomResponse> AddOffice(OfficeDTO officeDTO)
        {
            var office = OfficeMapper.OfficeDTOToOffice(officeDTO);
            var response = await _officeRepository.AddOffice(office);
            if(response == false)
                return new CustomResponse(false, "Adding Office Failed!");

            return new CustomResponse(true, "Successfully Added!");
        }

        public async Task<CustomResponse> DeleteOfficeById(string officeId)
        {
            var respose = await _officeRepository.DeleteOfficeById(officeId);
            if (respose == false)
                return new CustomResponse(false, "Deleting Office Failed!");

            return new CustomResponse(true, "Successfully Deleted!");
        }

        public async Task<List<OfficeTableInformationDTO>> TakeAllOffices()
        {
            var offices = await _officeRepository.TakeAllOffices();
            if (!offices.Any())
                return null;

            var officeDTOs = offices.Select(o=> OfficeMapper.OfficeToOfficeTableInformationDTO(o)).ToList();

            return officeDTOs;
        }

        public async Task<OfficeInformationDTO> TakeOfficeById(string officeId)
        {
            var office = await _officeRepository.TakeOfficeById(officeId);
            if (office is null)
                return null;

            var officeDTO = OfficeMapper.OfficeToOfficeInformationDTO(office);

            return officeDTO;
        }

        public async Task<CustomResponse> UpdateOffice(OfficeDTO officeDTO)
        {
            var office = OfficeMapper.OfficeDTOToOffice(officeDTO);
            var response = await _officeRepository.UpdateOffice(office);
            if (response == false)
                return new CustomResponse(false, "Updating Office Failed!");

            return new CustomResponse(true, "Successfully Updated!");
        }

        public async Task<CustomResponse> ChangeStatusOfOfficeById(string officeId)
        {
            var office = await _officeRepository.TakeOfficeById(officeId);
            if (office is null)
                return new CustomResponse(false, "Office not found!");

            office.Status = !office.Status;
            var response = await _officeRepository.UpdateOffice(office);
            if (response == false)
                return new CustomResponse(false,"Changing Office's status Failed!");

            return new CustomResponse(true, "Office's Status Successfully Changed!");
        }
    }
}
