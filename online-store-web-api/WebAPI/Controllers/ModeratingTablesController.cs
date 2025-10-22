using Microsoft.AspNetCore.Mvc;
using Core.Interfaces;
using Core.Models.ModeratingTable;
using Core.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModeratingTablesController(IModeratingTableService moderatingTableService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetModeratingTableDto>>> GetAll()
        {
            var moderatingTables = await moderatingTableService.GetAll();
            return Ok(moderatingTables);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetModeratingTableDto>> GetById(int id)
        {
            return Ok(await moderatingTableService.GetById(id));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> Create([FromForm] CreateModeratingTableDto createModeratingTableDto)
        {
            await moderatingTableService.Create(createModeratingTableDto);
            return Ok();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromForm] UpdateModeratingTableDto updateModeratingTableDto)
        {
            await moderatingTableService.Update(id, updateModeratingTableDto);
            return Ok();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await moderatingTableService.Delete(id);
            return Ok();
        }
    }
}
