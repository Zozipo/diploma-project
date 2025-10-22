using Core.Interfaces;
using Core.Models.Banner;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BannersController(IBannersService bannersService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await bannersService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await bannersService.GetById(id));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateBannerDto banner)
        {
            await bannersService.Create(banner);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromForm] UpdateBannerDto banner)
        {
            await bannersService.Update(id, banner);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await bannersService.Delete(id);
            return Ok();
        }
    }
}
