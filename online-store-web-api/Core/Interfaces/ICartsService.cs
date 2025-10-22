using Core.Models.Cart;

namespace Core.Interfaces
{
    public interface ICartsService
    {
        Task<IEnumerable<GetCartDto>> GetAll();
        Task<GetCartDto> GetById(int id);
        Task<GetCartDto> GetByUserId(string userId);
        Task<IEnumerable<GetCartItemDto>> GetCartItemsByUserId(string userId);
        Task Create(CreateCartDto cart);
        Task Delete(int id);
        Task DeleteProduct(int cartId, int productId);
        Task AddProduct(int productId, int cartId);
        Task RemoveProduct(int productId, int cartId);
    }

}
