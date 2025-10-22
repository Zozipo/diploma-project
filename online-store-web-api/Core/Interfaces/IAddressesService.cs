using Core.Models.Address;

namespace Core.Interfaces
{
    public interface IAddressesService
    {
        Task<IEnumerable<GetAddressDto>> GetAll();
        Task<GetAddressDto> GetById(int id);
        Task<GetAddressDto> GetByUserId(string userId);
        Task Create(CreateAddressDto address);
        Task Delete(int id);
        Task Update(int addressId, UpdateAddressDto address);
    }
}
    