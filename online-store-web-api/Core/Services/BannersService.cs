using Core.Interfaces;
using Core.Models.Banner;
using Core.Entities;
using AutoMapper;
using Core.Helpers;
using Core.Resources;
using System.Net;
using Core.Specification;

namespace Core.Services
{
    public class BannersService(IRepository<Banner> bannersRepo, 
                                IMapper mapper, 
                                IImagesService imageService) : IBannersService
    {
        public async Task<IEnumerable<GetBannerDto>> GetAll()
        {
            var banners = await bannersRepo.GetAll();
            return mapper.Map<IEnumerable<GetBannerDto>>(banners);
        }

        public async Task<GetBannerDto> GetById(int id)
        {
            Banner banner = await bannersRepo.GetBySpec(new Banners.ById(id))
                ?? throw new HttpException(ErrorMessages.BannerByIdNotFound, HttpStatusCode.NotFound);

            return mapper.Map<GetBannerDto>(banner);
        }

        public async Task<int> Create(CreateBannerDto banner)
        {
            var bannerEntity = mapper.Map<Banner>(banner);
            bannerEntity.Image = imageService.SaveImage(banner.Image);
            await bannersRepo.Insert(bannerEntity);
            await bannersRepo.Save();
            return bannerEntity.Id;
        }

        public async Task Update(int id, UpdateBannerDto banner)
        {
            Banner existingBanner = await bannersRepo.GetBySpec(new Banners.ById(id))
                ?? throw new HttpException(ErrorMessages.BannerByIdNotFound, HttpStatusCode.NotFound);

            mapper.Map(banner, existingBanner);

            if (banner.Image != null)
            {
                imageService.DeleteImage(existingBanner.Image);
                existingBanner.Image = imageService.SaveImage(banner.Image);
            }

            await bannersRepo.Update(existingBanner);
            await bannersRepo.Save();
        }

        public async Task Delete(int id)
        {
            Banner banner = await bannersRepo.GetBySpec(new Banners.ById(id))
                ?? throw new HttpException(ErrorMessages.BannerByIdNotFound, HttpStatusCode.NotFound);

            if (banner == null)
            {
                throw new HttpException(ErrorMessages.BannerByIdNotFound, HttpStatusCode.NotFound);
            }

            imageService.DeleteImage(banner.Image);
            await bannersRepo.Delete(id);
            await bannersRepo.Save();
        }
    }
}
