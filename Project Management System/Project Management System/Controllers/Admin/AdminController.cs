using Application.Interface.AdminInterface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Project_Management_System.Controllers.Admin
{
    [Route("api/admin")]
    [ApiController]
    public class AdminUserController : ControllerBase
    {
        private readonly IAdminUserService _service;

        public AdminUserController(IAdminUserService service)
        {
            _service = service;
        }


        [HttpGet("users")]
        public async Task<IActionResult> GetUsersByRoleAndDepartment(string role, string department)
        {
            var result = await _service.GetUsersByRoleAndDepartment(role, department);
            return Ok(result);
        }

        [HttpDelete("user/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _service.DeleteUser(id);
            return Ok(result);
        }
        [HttpGet("GetAllGroups")]
        public async Task<IActionResult> GetAllGroups(string department)
        {
            var result = await _service.GetAllGroups(department);
            return Ok(result);

        }
        [HttpDelete("group/{groupId}")]
        public async Task<IActionResult> DeleteGroup(int groupId)
        {
            var result = await _service.DeleteGroup(groupId);
            return result.Success ? Ok(result) : NotFound(result);
        }
        [HttpPatch("change-role/{id}")]
        public async Task<IActionResult> ChangeUserRole(int id, string newRole)
        {
            var result = await _service.ChangeUserRole(id, newRole);
            return Ok(result);
        }

        [HttpPatch("block-user/{id}")]
        public async Task<IActionResult> BlockUser(int id, bool block)
        {
            var result = await _service.UserBlockStatus(id, block);
            return Ok(result);
        }
    }
}
