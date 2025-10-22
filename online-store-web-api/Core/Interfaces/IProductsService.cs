using Core.Models;
using Core.Models.Product;

namespace Core.Interfaces
{
    public interface IProductsService
    {
        Task<IEnumerable<GetProductDto>> GetAll();
        Task<GetProductDto?> GetById(int id);
        Task Create(CreateProductDto product);
        Task Update(int productId, UpdateProductDto product);
        Task<IEnumerable<GetProductPreviewDto>> GetProductsByCategory(int categoryId);
        Task Delete(int id);
        Task<IEnumerable<GetProductPreviewDto>> GetFrequentlyBoughtTogether(int productId);
        Task<PaginatedList<GetProductDto>>  GetByFilter(ItemsFilter filter);
        Task<IEnumerable<GetProductPreviewDto>> GetPromotionalOffers(int count);
        Task<IEnumerable<GetProductPreviewDto>> GetPromotionalOffersWithFavorites(int count, string userId);
        Task<IEnumerable<GetProductPreviewDto>> GetPopularProducts(int count);
        Task<IEnumerable<GetProductPreviewDto>> GetPopularProductsWithFavorites(int count, string userId);
        Task<IEnumerable<GetProductPreviewDto>> GetPopularProductsByUserId(string userId, int count);
        Task<IEnumerable<GetProductPreviewDto>> GetPopularProductsByCategory(int categoryId, int count);
    }
}
