using Core.Entities;
using Core.Models.Product;
using Core.Models.User;

namespace Core.Interfaces
{
    public interface IFavoriteProductsService
    {
        Task<IEnumerable<FavoriteProduct>> GetAll();
        Task<FavoriteProduct?> GetById(int id);
        Task<bool> UserHasFavoriteProduct(string userId, int productId);
        Task<IEnumerable<GetProductPreviewDto>> GetFavoriteProductsByUserId(string userId);
        Task<IEnumerable<GetUserDto>> GetUsersByFavoriteProductId(int productId);
        Task AddToFavorites(string userId, int productId);
        Task RemoveFromFavorites(string userId, int productId);
        Task ToggleFavorite(string userId, int productId);
        Task Delete(int id);
    }
}