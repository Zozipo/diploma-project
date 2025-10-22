using Core.Models.Order;

namespace Core.Interfaces
{
    public interface IOrdersService
    {
        Task Create(CreateOrderDto model);
        Task<IEnumerable<GetOrderDto>> GetAll();
        Task<IEnumerable<GetOrderDto>> GetByUserId(string userId);
        Task<GetOrderDto> GetById(int id);
        Task ChangeStatus(int id);
    }
}
