using Microsoft.AspNetCore.Mvc;
using OfficesAPI.Services.Abstractions.Interfaces;
using OfficesAPI.Shared.DTOs;

namespace OfficesAPI.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfficeController : ControllerBase
    {
        private readonly IOfficeServices _officeServices;

        public OfficeController(IOfficeServices officeServices)
        {
            _officeServices = officeServices;
        }


        [HttpGet]
        public async Task<IActionResult> TakeOffices()
        {
            var result = await _officeServices.TakeAllOffices();
            if (!result.Any())
                return NotFound("No Offices Found!");
            return Ok(result);
        }

        [HttpGet("{officeId}")]
        public async Task<IActionResult> TakeOfficeById(string officeId)
        {
            var result = await _officeServices.TakeOfficeById(officeId);
            if (result == null)
                return NotFound("Office Not found!");
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddOffice([FromBody] OfficeDTO officeDTO)
        {
            var result = await _officeServices.AddOffice(officeDTO);
            if (result.Flag == false)
                return StatusCode(500, result.Message);
            return Ok(result.Message);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateOffice([FromBody] OfficeDTO officeDTO)
        {
            var result = await _officeServices.UpdateOffice(officeDTO);
            if (result.Flag == false)
                return StatusCode(500, result.Message);
            return Ok(result.Message);
        }

        [HttpDelete("{officeId}")]
        public async Task<IActionResult> DeleteOfficeById(string officeId)
        {
            var result = await _officeServices.DeleteOfficeById(officeId);
            if (result.Flag == false)
                return StatusCode(500, result.Message);
            return Ok(result.Message);
        }

        [HttpPatch("/changestatusofofficebyid")]
        public async Task<IActionResult> ChangeStatusOfOfficeById([FromBody] string officeId)
        {
            var result = await _officeServices.ChangeStatusOfOfficeById(officeId);
            if (result.Flag == false)
                return StatusCode(500, result.Message);
            return Ok(result.Message);
        }

        //[HttpPatch("/changestatusofofficebyid")]
        //public async Task<IActionResult> ChangeStatusOfOfficeById([FromBody]string officeId)
        //{
        //    var result = await _officeServices.ChangeStatusOfOfficeById(officeId);
        //    if (result.Flag == false)
        //        return StatusCode(500, result.Message);
        //    return Ok(result.Message);
        //}

    }
}
