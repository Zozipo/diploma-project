using Core.Entities;
using Core.Models.Product;

namespace Core.Interfaces
{
    public interface IViewedProductsService
    {
        Task<IEnumerable<ViewedProduct>> GetAll();
        Task<ViewedProduct> GetById(int id);
        Task<IEnumerable<GetProductPreviewDto>> GetViewedProductsByUserId(string userId, int count);
        Task Add(string userId, int productId);
        Task Remove(string userId, int productId);
        Task Delete(int id);
    }
}