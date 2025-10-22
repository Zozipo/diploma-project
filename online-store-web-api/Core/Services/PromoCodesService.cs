using Core.Interfaces;
using Core.Entities;
using AutoMapper;
using Core.Helpers;
using Core.Resources;
using System.Net;
using Core.Models.Promocode;
using Core.Models.PromoCode;
using Core.Specification;
using Microsoft.AspNetCore.Identity;

namespace Core.Services
{
    public class PromoCodesService(IRepository<PromoCode> promoCodesRepo,
                                   IMapper mapper,
                                   UserManager<User> userManager) : IPromoCodesService
    {
        public async Task<IEnumerable<GetPromoCodeDto>> GetAll()
        {
            var promoCodes = await promoCodesRepo.GetAll();
            return mapper.Map<IEnumerable<GetPromoCodeDto>>(promoCodes);
        }

        public async Task<GetPromoCodeDto> GetById(int id)
        {
            var promoCode = await promoCodesRepo.GetBySpec(new PromoCodes.ById(id)) ??
                            throw new HttpException(ErrorMessages.PromoCodeByIdNotFound, HttpStatusCode.NotFound);

            return mapper.Map<GetPromoCodeDto>(promoCode);
        }

        public async Task Create(CreatePromoCodeDto promoCodeDto)
        {
            await promoCodesRepo.Insert(mapper.Map<PromoCode>(promoCodeDto));
            await promoCodesRepo.Save();
        }

        public async Task<string> Activate(string promoCode, string userId)
        {
            var user = await userManager.FindByIdAsync(userId) ??
                       throw new HttpException(ErrorMessages.UserByIdNotFound, HttpStatusCode.NotFound); ;

            if (user.PromoCodeForNextUseId != null)
            {
                return ErrorMessages.UserAlreadyActivatedPromoCode;
            }

            var promoCodeEntity = await promoCodesRepo.GetBySpec(new PromoCodes.ByCode(promoCode));

            if (promoCodeEntity == null)
            {
                return ErrorMessages.PromoCodeByCodeNotFound;
            }

            if (promoCodeEntity.RemainingUses.HasValue && promoCodeEntity.RemainingUses.Value > 0)
            {
                if (promoCodeEntity.RemainingUses == 0)
                {
                    return "Promo code has no remaining uses";
                }

                user.PromoCodeForNextUseId = promoCodeEntity.Id;

                promoCodeEntity.RemainingUses -= 1;

                await promoCodesRepo.Update(promoCodeEntity);
                await userManager.UpdateAsync(user);
            }
            else
            {
                user.PromoCodeForNextUseId = promoCodeEntity.Id;
                await userManager.UpdateAsync(user);
            }
            return null;
        }

        public async Task Update(int id, UpdatePromoCodeDto promoCodeDto)
        {
            var existingPromoCode = await promoCodesRepo.GetBySpec(new PromoCodes.ById(id)) ??
                                    throw new HttpException(ErrorMessages.PromoCodeByIdNotFound,
                                        HttpStatusCode.NotFound);
            mapper.Map(promoCodeDto, existingPromoCode);

            await promoCodesRepo.Update(existingPromoCode);
            await promoCodesRepo.Save();
        }

        public async Task Delete(int id)
        {
            _ = await promoCodesRepo.GetBySpec(new PromoCodes.ById(id)) ??
                throw new HttpException(ErrorMessages.PromoCodeByIdNotFound, HttpStatusCode.NotFound);

            await promoCodesRepo.Delete(id);
            await promoCodesRepo.Save();
        }
    }
}
