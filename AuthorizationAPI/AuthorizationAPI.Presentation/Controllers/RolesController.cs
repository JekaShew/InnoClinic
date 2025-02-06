using AuthorizationAPI.Services.Abstractions.Interfaces;
using AuthorizationAPI.Shared.DTOs.RoleDTOs;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationAPI.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RolesController : ResponseMessageHandler
{
    private readonly IRoleService _roleService;
    public RolesController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpGet("{roleId}")]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> GetRoleById(Guid roleId)
    {
        var result = await _roleService.GetRoleByIdAsync(roleId);
        if (result.Flag == false)
            return HandleResponseMessage(result);
        return Ok(result);
    }

    [HttpGet]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> GetAllRoles()
    {
        var result = await _roleService.GetAllRolesAsync();
        if (result.Flag == false)
            return HandleResponseMessage(result);
        return Ok(result);
    }

    [HttpPost]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> AddRole([FromBody] RoleForCreateDTO roleForCreateDTO)
    {
        var result = await _roleService.CreateRoleAsync(roleForCreateDTO);
        if (result.Flag == false)
            return HandleResponseMessage(result);
        return Ok(result);
    }

    [HttpPut("{roleId}")]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> UpdateRole(Guid roleId, [FromBody] RoleForUpdateDTO roleForUpdateDTO)
    {
        var result = await _roleService.UpdateRoleAsync(roleId, roleForUpdateDTO);
        if (result.Flag == false)
            return HandleResponseMessage(result);
        return Ok(result);
    }

    [HttpDelete("{roleId}")]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteRoleById(Guid roleId)
    {
        var result = await _roleService.DeleteRoleByIdAsync(roleId);
        if (result.Flag == false)
            return  HandleResponseMessage(result);
        return Ok(result);
    }
}
