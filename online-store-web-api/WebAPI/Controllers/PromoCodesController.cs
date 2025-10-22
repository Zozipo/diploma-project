using Core.Interfaces;
using Core.Models.Promocode;
using Microsoft.AspNetCore.Mvc;
using Core.Models.PromoCode;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromoCodesController(IPromoCodesService promoCodesService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var promoCodes = await promoCodesService.GetAll();
            return Ok(promoCodes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var promoCode = await promoCodesService.GetById(id);
            return Ok(promoCode);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePromoCodeDto promoCodeDto)
        {
            await promoCodesService.Create(promoCodeDto);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePromoCodeDto promoCodeDto)
        {
            await promoCodesService.Update(id, promoCodeDto);
            return Ok();
        }

        [HttpPost("activate/{promoCode}/{userId}")]
        public async Task<IActionResult> Activate(string promoCode, string userId)
        {
            var errorMessage = await promoCodesService.Activate(promoCode, userId);
            if (errorMessage != null)
            {
                return BadRequest(errorMessage);
            }
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await promoCodesService.Delete(id);
            return Ok();
        }
    }
}
