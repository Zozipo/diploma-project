using Core.Models.Banner;

namespace Core.Interfaces
{
    public interface IBannersService
    {
        Task<IEnumerable<GetBannerDto>> GetAll();
        Task<GetBannerDto> GetById(int id);
        Task<int> Create(CreateBannerDto banner);
        Task Update(int bannerId, UpdateBannerDto banner);
        Task Delete(int id);
    }
}
