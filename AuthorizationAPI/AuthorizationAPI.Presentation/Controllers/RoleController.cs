using AuthorizationAPI.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AuthorizationAPI.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {

        public RoleController()
        {

        }

        [HttpGet("{roleId}")]
        public async Task<IActionResult> TakeRoleById(Guid roleId)
        {
            var result = await _mediator.Send(new TakeRoleDTOByIdQuery() { Id = roleId });
            if (result == null)
                return NotFound("Role Not found!");
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> TakeRoles()
        {
            var result = await _mediator.Send(new TakeRoleDTOListQuery() { });
            if (!result.Any())
                return NotFound("No roles Found!");
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddRole([FromBody] RoleDTO roleDTO)
        {
            var result = await _mediator.Send(new AddRoleCommand() { RoleDTO = roleDTO });
            if (result.Flag == false)
                return StatusCode(500, result.Message);
            return Ok(result.Message);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateRole([FromBody] RoleDTO roleDTO)
        {
            var result = await _mediator.Send(new UpdateRoleCommand() { RoleDTO = roleDTO });
            if (result.Flag == false)
                return StatusCode(500, result.Message);
            return Ok(result.Message);
        }

        [HttpDelete("{roleId}")]
        public async Task<IActionResult> DeleteRoleById(Guid roleId)
        {
            var result = await _mediator.Send(new DeleteRoleByIdCommand() { Id = roleId });
            if (result.Flag == false)
                return StatusCode(500, result.Message);
            return Ok(result.Message);
        }
    }
}
