using Core.Entities;
using Core.Interfaces;
using Core.Models.Permission;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public class TablePermissionsController(ITablePermissionsService permissionsService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TablePermission>>> Get()
        {
            return Ok(await permissionsService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TablePermission>> Get([FromRoute] int id)
        {
            return Ok(await permissionsService.GetById(id));
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<TablePermission>> GetByUserId([FromRoute] string userId)
        {
            return Ok(await permissionsService.GetByUserId(userId));
        }

        [HttpPut("replace/{userId}")]
        public async Task<IActionResult> Replace([FromRoute] string userId, [FromBody] List<int> moderatingTablesIds)
        {
            await permissionsService.Replace(userId, moderatingTablesIds);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTablePermissionDto createPermissionDto)
        {
            await permissionsService.Create(createPermissionDto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await permissionsService.Delete(id);
            return Ok();
        }
    }
}
