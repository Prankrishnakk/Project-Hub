using Application.Interface.HodInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Project_Management_System.Controllers.Hod
{
    [Route("api/[controller]")]
    [ApiController]
    public class TutorGroupController : ControllerBase
    {
        private readonly IGetDepartmentGroupsService _departmentGroupsService;

        public TutorGroupController(IGetDepartmentGroupsService departmentGroupsService)
        {
            _departmentGroupsService = departmentGroupsService;
        }

        // GET: api/hod/groups/{department}
        [Authorize(Roles = "HOD")]
        [HttpGet("groups/{department}")]
        public async Task<IActionResult> GetGroupsByDepartment(string department)
        {
            var result = await _departmentGroupsService.GetGroupsByDepartmentAsync(department);

            if (result == null || result.Count == 0)
                return NotFound($"No project groups found for department: {department}");

            return Ok(result);
        }
    }
}


