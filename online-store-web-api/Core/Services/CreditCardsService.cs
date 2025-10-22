using AutoMapper;
using Core.Entities;
using Core.Helpers;
using Core.Interfaces;
using Core.Models.CreditCard;
using Core.Resources;
using Core.Specification;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace Core.Services
{
    public class CreditCardsService(UserManager<User> userManager,
                                    IRepository<CreditCard> creditCardsRepo,
                                    IMapper mapper) : ICreditCardService
    {
        public async Task Create(CreateCreditCardDto creditCardDto)
        {
            _ = await userManager.FindByIdAsync(creditCardDto.UserId)
                ?? throw new HttpException(ErrorMessages.UserByIdNotFound, HttpStatusCode.NotFound);
            var creditCard = mapper.Map<CreditCard>(creditCardDto);
            await creditCardsRepo.Insert(creditCard);
            await creditCardsRepo.Save();
        }

        public async Task Delete(int id)
        {
            _ = await creditCardsRepo.GetByID(id) 
                ?? throw new HttpException(ErrorMessages.CreditCardByIdNotFound, HttpStatusCode.NotFound);

            await creditCardsRepo.Delete(id);
            await creditCardsRepo.Save();
        }

        public async Task<IEnumerable<GetCreditCardDto>> GetAll()
        {
            var creditCards = await creditCardsRepo.GetAllBySpec(new CreditCards.All());

            return mapper.Map<IEnumerable<GetCreditCardDto>>(creditCards);
        }

        public async Task<GetCreditCardDto> GetById(int id)
        {
            CreditCard creditCard = await creditCardsRepo.GetBySpec(new CreditCards.ById(id))
                ?? throw new HttpException(ErrorMessages.CreditCardByIdNotFound, HttpStatusCode.NotFound);

            return mapper.Map<GetCreditCardDto>(creditCard);
        }

        public async Task<GetCreditCardDto> GetByUserId(string userId)
        {
            CreditCard creditCard = await creditCardsRepo.GetBySpec(new CreditCards.ByUserId(userId))
                ?? throw new HttpException(ErrorMessages.CreditCardByIdNotFound, HttpStatusCode.NotFound);

            return mapper.Map<GetCreditCardDto>(creditCard);
        }
    }
}
