using System.Net;
using AutoMapper;
using Core.Entities;
using Core.Helpers;
using Core.Interfaces;
using Core.Models.Product;
using Core.Resources;
using Core.Specification;
using Microsoft.AspNetCore.Identity;

namespace Core.Services
{
    public class ViewedProductsService(IRepository<ViewedProduct> viewedProductsRepo,
                                       IRepository<Product> productsRepo,
                                       IRepository<FavoriteProduct> favoriteProductsRepo,
                                       UserManager<User> userManager, IMapper mapper) : IViewedProductsService
    {
        public async Task<IEnumerable<ViewedProduct>> GetAll()
        {
            var viewedProducts = await viewedProductsRepo.GetAllBySpec(new ViewedProducts.All());
            return viewedProducts;
        }

        public async Task<ViewedProduct> GetById(int id)
        {
            var viewedProduct = await viewedProductsRepo
                                            .GetBySpec(new ViewedProducts.ById(id)) ??
                                        throw new HttpException(ErrorMessages.ViewedProductByIdNotFound,
                                            HttpStatusCode.NotFound);

            return viewedProduct;
        }

        public async Task<IEnumerable<GetProductPreviewDto>> GetViewedProductsByUserId(string userId, int count)
        {
            var user = await userManager.FindByIdAsync(userId) ??
                       throw new HttpException(ErrorMessages.UserByIdNotFound, HttpStatusCode.NotFound);

            var userViewedProducts = await viewedProductsRepo.GetAllBySpec(new ViewedProducts.ByUserId(userId));
            userViewedProducts = userViewedProducts.Take(count);

            var viewedProductsDto = mapper.Map<IEnumerable<GetProductPreviewDto>>(userViewedProducts.Select(ufp => ufp.Product));

            var userFavoriteProducts = await favoriteProductsRepo.GetAllBySpec(new FavoriteProducts.ByUserId(userId));
            var userFavoriteProductIds = userFavoriteProducts.Select(fp => fp.ProductId);

            foreach (var productDto in viewedProductsDto)
            {
                productDto.IsFavorite = userFavoriteProductIds.Contains(productDto.Id);
            }

            return viewedProductsDto;
        }

        public async Task Add(string userId, int productId)
        {
            _ = await userManager
                    .FindByIdAsync(userId) ??
                throw new HttpException(ErrorMessages.UserByIdNotFound, HttpStatusCode.NotFound);
            _ = await productsRepo
                    .GetBySpec(new Products.ById(productId)) ??
                throw new HttpException(ErrorMessages.ProductByIdNotFound, HttpStatusCode.NotFound);

            var existingRelationship = await viewedProductsRepo
                .GetBySpec(new ViewedProducts.ByUserAndProductIds(userId, productId));

            if (existingRelationship != null)
            {
                existingRelationship.ViewedAt = DateTime.Now.ToUniversalTime();
                await viewedProductsRepo.Update(existingRelationship);
            }
            else
            {
                var userViewedProducts = await viewedProductsRepo
                    .GetAllBySpec(new ViewedProducts.ByUserId(userId));

                if (userViewedProducts.Count() >= 10)
                {
                    var oldestRelationship = userViewedProducts
                        .OrderBy(rvp => rvp.ViewedAt)
                        .First();

                    await viewedProductsRepo.Delete(oldestRelationship);
                }

                var newRelationship = new ViewedProduct
                {
                    UserId = userId,
                    ProductId = productId,
                    ViewedAt = DateTime.Now.ToUniversalTime()
                };

                await viewedProductsRepo.Insert(newRelationship);
            }

            await viewedProductsRepo.Save();
        }

        public async Task Remove(string userId, int productId)
        {
            _ = await userManager
                    .FindByIdAsync(userId) ??
                throw new HttpException(ErrorMessages.UserByIdNotFound, HttpStatusCode.NotFound);
            _ = await productsRepo
                    .GetBySpec(new Products.ById(productId)) ??
                throw new HttpException(ErrorMessages.ProductByIdNotFound, HttpStatusCode.NotFound);

            var existingRelationship = await viewedProductsRepo
                                           .GetBySpec(new ViewedProducts.ByUserAndProductIds(userId,
                                               productId)) ??
                                       throw new HttpException(ErrorMessages.ViewedProductByIdNotFound,
                                           HttpStatusCode.NotFound);

            await viewedProductsRepo.Delete(existingRelationship.Id);
            await viewedProductsRepo.Save();
        }

        public async Task Delete(int id)
        {
            _ = await viewedProductsRepo
                    .GetBySpec(new ViewedProducts.ById(id)) ??
                throw new HttpException(ErrorMessages.ViewedProductByIdNotFound,
                    HttpStatusCode.NotFound);

            await viewedProductsRepo.Delete(id);
            await viewedProductsRepo.Save();
        }
    }
}
