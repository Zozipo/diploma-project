using Core.Interfaces;
using Core.Models.CreditCard;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreditCardController(ICreditCardService creditCardService) : ControllerBase
    {
        [HttpGet("getAll")]
        public async Task<ActionResult<IEnumerable<GetCreditCardDto>>> Get()
        {
            return Ok(await creditCardService.GetAll());
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<GetCreditCardDto>> GetById(int id)
        {
            return Ok(await creditCardService.GetById(id));
        }
        [HttpGet("getByUserId/{userId}")]
        public async Task<ActionResult<GetCreditCardDto>> GetByUserId(string userId)
        {
            return Ok(await creditCardService.GetByUserId(userId));
        }
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateCreditCardDto createCreditCardDto)
        {
            await creditCardService.Create(createCreditCardDto);
            return Ok();
        }
    }
}
