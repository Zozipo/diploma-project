using Core.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViewedProductsController(IViewedProductsService viewedProductsService)
        : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await viewedProductsService.GetAll();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await viewedProductsService.GetById(id);
            return Ok(product);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("getViewedProductsByUserId/{userId}/{count}")]
        public async Task<IActionResult> GetViewedProductsByUserId(string userId, int count)
        {
            var products = await viewedProductsService.GetViewedProductsByUserId(userId, count);
            return Ok(products);
        }

        [HttpPost("{userId}/{productId}")]
        public async Task<IActionResult> Add(string userId, int productId)
        {
            await viewedProductsService.Add(userId, productId);
            return Ok();
        }

        [HttpDelete("{userId}/{productId}")]
        public async Task<IActionResult> Remove(string userId, int productId)
        {
            await viewedProductsService.Remove(userId, productId);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await viewedProductsService.Delete(id);
            return Ok();
        }
    }
}
