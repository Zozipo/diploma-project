using Core.Interfaces;
using Core.Models.Address;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController(IAddressesService addressService) : ControllerBase
    {
        [HttpGet("getAll")]
        public async Task<ActionResult<IEnumerable<GetAddressDto>>> Get()
        {
            return Ok(await addressService.GetAll());
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<GetAddressDto>> GetById(int id)
        {
            return Ok(await addressService.GetById(id));
        }
        [HttpGet("getByUserId/{userId}")]
        public async Task<ActionResult<GetAddressDto>> GetByUserId(string userId)
        {
            return Ok(await addressService.GetByUserId(userId));
        }
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateAddressDto addressDto)
        {
            await addressService.Create(addressDto);
            return Ok();
        }
        [HttpPut]
        public async Task<ActionResult> Update(int addressId, [FromBody] UpdateAddressDto editAddressDto)
        {
            await addressService.Update(addressId, editAddressDto);
            return Ok();
        }
    }
}
