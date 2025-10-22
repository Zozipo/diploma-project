using Core.Models.CreditCard;

namespace Core.Interfaces
{
    public interface ICreditCardService
    {
        Task<IEnumerable<GetCreditCardDto>> GetAll();
        Task<GetCreditCardDto> GetById(int id);
        Task<GetCreditCardDto> GetByUserId(string userId);
        Task Create(CreateCreditCardDto creditCard);
        Task Delete(int id);
    }
}
