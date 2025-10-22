using Core.Interfaces;
using Core.Models.Order;
using Microsoft.AspNetCore.Mvc;


namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController(IOrdersService orderService) : ControllerBase
    {
        [HttpGet("getAll")]
        public async Task<ActionResult<IEnumerable<GetOrderDto>>> Get()
        {
            return Ok(await orderService.GetAll());
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<GetOrderDto>> GetById(int id)
        {
            return Ok(await orderService.GetById(id));
        }
        [HttpGet("getByUserId/{userId}")]
        public async Task<ActionResult<GetOrderDto>> GetByUserId(string userId)
        {
            return Ok(await orderService.GetByUserId(userId));
        }
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateOrderDto createOrderDto)
        {
            await orderService.Create(createOrderDto);
            return Ok();
        }
        [HttpPost("{id}")]
        public async Task<ActionResult> ChangeOrderStatus(int id)
        {
            await orderService.ChangeStatus(id);
            return Ok();
        }
    }
}
