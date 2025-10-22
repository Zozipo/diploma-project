using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController(IRolesService rolesService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] string roleName)
        {
            await rolesService.Create(roleName);
            return Ok();
        }

        [HttpPost("addToRole")]
        public async Task<IActionResult> AddToRole(string userId, string roleName)
        {
            await rolesService.AddToRole(userId, roleName);
            return Ok();
        }

        [HttpPost("removeFromRole")]
        public async Task<IActionResult> RemoveFromRole(string userId, string roleName)
        {
            await rolesService.RemoveFromRole(userId, roleName);
            return Ok();
        }

        [HttpDelete("{roleName}")]
        public async Task<IActionResult> Delete([FromRoute] string roleName)
        {
            await rolesService.Delete(roleName);
            return Ok();
        }

        [HttpGet("getUserRoles/{userId}")]
        public async Task<IActionResult> GetUserRoles([FromRoute] string userId)
        {
            return Ok(await rolesService.GetUserRoles(userId));
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await rolesService.GetAll());
        }
    }
}
