using Core.Interfaces;
using Core.Models.Cart;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController(ICartsService cartService) : ControllerBase
    {
        [HttpGet("getAll")]
        public async Task<ActionResult<IEnumerable<GetCartDto>>> Get()
        {
            return Ok(await cartService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetCartDto>> GetById(int id)
        {
            return Ok(await cartService.GetById(id));
        }

        [HttpGet("getByUserId/{userId}")]
        public async Task<ActionResult<GetCartDto>> GetByUserId(string userId)
        {
            return Ok(await cartService.GetByUserId(userId));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("getCartItemsByUserId/{userId}")]
        public async Task<ActionResult<GetCartDto>> GetCartItemsByUserId(string userId)
        {
            return Ok(await cartService.GetCartItemsByUserId(userId));
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateCartDto createCartDto)
        {
            await cartService.Create(createCartDto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await cartService.Delete(id);
            return Ok();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("deleteProduct")]
        public async Task<ActionResult> DeleteProduct(int cartId, int productId)
        {
            await cartService.DeleteProduct(cartId, productId);
            return Ok();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("addProduct")]
        public async Task<IActionResult> AddProduct(int cartId, int productId)
        {
            await cartService.AddProduct(productId, cartId);
            return Ok();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("removeProduct")]
        public async Task<IActionResult> RemoveProduct(int cartId, int productId)
        {
            await cartService.RemoveProduct(productId, cartId);
            return Ok();
        }
    }
}
