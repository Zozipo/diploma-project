using Core.Entities;
using Core.Models.Order;

namespace Core.Interfaces
{
    public interface IOrderHistoryService
    {
        Task<IEnumerable<OrderHistory>> GetAll();
        Task<OrderHistory> GetById(int id);
        Task<IEnumerable<GetOrderPreviewDto>> GetOrderHistoryByUserId(string userId);
        Task Add(string userId, int orderId);
        Task Remove(string userId, int orderId);
        Task Delete(int id);
    }
}
