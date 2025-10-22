using Core.Models.Promocode;
using Core.Models.PromoCode;

namespace Core.Interfaces
{
    public interface IPromoCodesService
    {
        Task<IEnumerable<GetPromoCodeDto>> GetAll();
        Task<GetPromoCodeDto> GetById(int id);
        Task Create(CreatePromoCodeDto promoCodeDto);
        Task<string> Activate(string promoCode, string userId);
        Task Update(int id, UpdatePromoCodeDto promoCodeDto);
        Task Delete(int id);
    }
}
