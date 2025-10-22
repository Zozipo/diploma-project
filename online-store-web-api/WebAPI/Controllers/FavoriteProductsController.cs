using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoriteProductsController(IFavoriteProductsService favoriteProductsService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userFavoriteProducts = await favoriteProductsService.GetAll();
            return Ok(userFavoriteProducts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FavoriteProduct>> GetById(int id)
        {
            return Ok(await favoriteProductsService.GetById(id));
        }

        [HttpGet("getFavoriteProductsByUserId/{userId}")]
        public async Task<IActionResult> GetFavoriteProductsByUserId(string userId)
        {
            var favoriteProducts = await favoriteProductsService.GetFavoriteProductsByUserId(userId);
            return Ok(favoriteProducts);
        }

        [HttpGet("getUsersByFavoriteProductId/{productId}")]
        public async Task<IActionResult> GetUsersByFavoriteProductId(int productId)
        {
            var favoriteUsers = await favoriteProductsService.GetUsersByFavoriteProductId(productId);
            return Ok(favoriteUsers);
        }

        [HttpPost("{userId}/{productId}")]
        public async Task<IActionResult> AddToFavorites(string userId, int productId)
        {
            await favoriteProductsService.AddToFavorites(userId, productId);
            return Ok();
        }

        [HttpDelete("{userId}/{productId}")]
        public async Task<IActionResult> RemoveFromFavorites(string userId, int productId)
        {
            await favoriteProductsService.RemoveFromFavorites(userId, productId);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await favoriteProductsService.Delete(id);
            return Ok();
        }

        [HttpGet("IsFavorite/{userId}/{productId}")]
        public async Task<ActionResult<bool>> IsFavoriteProduct(string userId, int productId)
        {
            var isFavorite = await favoriteProductsService.UserHasFavoriteProduct(userId, productId);
            return Ok(isFavorite);
        }

        [HttpPost("toggleFavorite/{userId}/{productId}")]
        public async Task<IActionResult> ToggleFavorite(string userId, int productId)
        {
            await favoriteProductsService.ToggleFavorite(userId, productId);
            return Ok();
        }
    }
}
