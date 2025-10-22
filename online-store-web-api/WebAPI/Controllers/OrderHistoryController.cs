using Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderHistoryController(OrderHistoryService orderHistoryService)
        : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orders = await orderHistoryService.GetAll();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await orderHistoryService.GetById(id);
            return Ok(order);
        }

        [HttpGet("getOrderHistoryByUserId/{userId}")]
        public async Task<IActionResult> GetViewedProductsByUserId(string userId)
        {
            var orders = await orderHistoryService.GetOrderHistoryByUserId(userId);
            return Ok(orders);
        }

        [HttpPost("{userId}/{orderId}")]
        public async Task<IActionResult> Add(string userId, int orderId)
        {
            await orderHistoryService.Add(userId, orderId);
            return Ok();
        }

        [HttpDelete("{userId}/{orderId}")]
        public async Task<IActionResult> Remove(string userId, int orderId)
        {
            await orderHistoryService.Remove(userId, orderId);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await orderHistoryService.Delete(id);
            return Ok();
        }
    }
}
