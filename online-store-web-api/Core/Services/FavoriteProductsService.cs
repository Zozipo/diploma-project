using System.Net;
using AutoMapper;
using Core.Entities;
using Core.Helpers;
using Core.Interfaces;
using Core.Models.Product;
using Core.Models.User;
using Core.Resources;
using Core.Specification;
using Microsoft.AspNetCore.Identity;

namespace Core.Services
{
    public class FavoriteProductsService(UserManager<User> userManager,
                                         IRepository<Product> productsRepo,
                                         IRepository<FavoriteProduct> favoriteProductsRepo,
                                         IMapper mapper) : IFavoriteProductsService
    {
        public async Task<IEnumerable<FavoriteProduct>> GetAll()
        {
            var favoriteProducts = await favoriteProductsRepo.GetAllBySpec(new FavoriteProducts.All());
            return favoriteProducts;
        }

        public async Task<FavoriteProduct> GetById(int id)
        {
            var favoriteProduct = await favoriteProductsRepo.GetBySpec(new FavoriteProducts.ById(id)) ??
                                      throw new HttpException(ErrorMessages.FavoriteProductByIdNotFound,
                                          HttpStatusCode.NotFound);

            return favoriteProduct;
        }

        public async Task<IEnumerable<GetProductPreviewDto>> GetFavoriteProductsByUserId(string userId)
        {
            _ = await userManager
                    .FindByIdAsync(userId) ??
                throw new HttpException(ErrorMessages.UserByIdNotFound, HttpStatusCode.NotFound);

            var favoriteProducts =
                await favoriteProductsRepo.GetAllBySpec(new FavoriteProducts.ByUserId(userId));

            return mapper.Map<IEnumerable<GetProductPreviewDto>>(favoriteProducts.Select(ufp => ufp.Product));
        }

        public async Task<IEnumerable<GetUserDto>> GetUsersByFavoriteProductId(int productId)
        {
            _ = await productsRepo
                    .GetByID(productId) ??
                throw new HttpException(ErrorMessages.ProductByIdNotFound, HttpStatusCode.NotFound);

            var favoriteProducts =
                await favoriteProductsRepo.GetAllBySpec(new FavoriteProducts.ByProductId(productId));

            return mapper.Map<IEnumerable<GetUserDto>>(favoriteProducts.Select(ufp => ufp.User));
        }

        public async Task AddToFavorites(string userId, int productId)
        {
            _ = await userManager
                    .FindByIdAsync(userId) ??
                throw new HttpException(ErrorMessages.UserByIdNotFound, HttpStatusCode.NotFound);
            _ = await productsRepo
                    .GetBySpec(new Products.ById(productId)) ??
                throw new HttpException(ErrorMessages.ProductByIdNotFound, HttpStatusCode.NotFound);

            var existingFavoriteProduct =
                await favoriteProductsRepo.GetBySpec(
                    new FavoriteProducts.ByUserAndProductIds(userId, productId));

            if (existingFavoriteProduct != null)
                throw new HttpException(ErrorMessages.FavoriteProductAlreadyExist, HttpStatusCode.Conflict);

            var favoriteProduct = new FavoriteProduct
            {
                UserId = userId,
                ProductId = productId,
            };

            await favoriteProductsRepo.Insert(favoriteProduct);
            await favoriteProductsRepo.Save();
        }

        public async Task RemoveFromFavorites(string userId, int productId)
        {
            _ = await userManager
                    .FindByIdAsync(userId) ??
                throw new HttpException(ErrorMessages.UserByIdNotFound, HttpStatusCode.NotFound);
            _ = await productsRepo
                    .GetBySpec(new Products.ById(productId)) ??
                throw new HttpException(ErrorMessages.ProductByIdNotFound, HttpStatusCode.NotFound);

            var favoriteProduct = await favoriteProductsRepo
                                          .GetBySpec(new FavoriteProducts.ByUserAndProductIds(userId, productId)) ??
                                      throw new HttpException(ErrorMessages.FavoriteProductByIdNotFound,
                                          HttpStatusCode.NotFound);

            await favoriteProductsRepo.Delete(favoriteProduct.Id);
            await favoriteProductsRepo.Save();
        }

        public async Task<bool> UserHasFavoriteProduct(string userId, int productId)
        {
            var favoriteProduct = await favoriteProductsRepo.GetBySpec(new FavoriteProducts.ByUserAndProductIds(userId, productId));
            return favoriteProduct != null;
        }

        public async Task Delete(int id)
        {
            _ = await favoriteProductsRepo
                .GetByID(id) ?? throw new HttpException(ErrorMessages.FavoriteProductByIdNotFound,
                HttpStatusCode.NotFound);

            await favoriteProductsRepo.Delete(id);
            await favoriteProductsRepo.Save();
        }

        public async Task ToggleFavorite(string userId, int productId)
        {
            _ = await userManager.FindByIdAsync(userId) ??
                throw new HttpException(ErrorMessages.UserByIdNotFound, HttpStatusCode.NotFound);

            _ = await productsRepo.GetBySpec(new Products.ById(productId)) ??
                throw new HttpException(ErrorMessages.ProductByIdNotFound, HttpStatusCode.NotFound);

            var existingFavoriteProduct =
                await favoriteProductsRepo.GetBySpec(new FavoriteProducts.ByUserAndProductIds(userId, productId));

            if (existingFavoriteProduct != null)
            {
                await favoriteProductsRepo.Delete(existingFavoriteProduct.Id);
            }
            else
            {
                var favoriteProduct = new FavoriteProduct
                {
                    UserId = userId,
                    ProductId = productId,
                };

                await favoriteProductsRepo.Insert(favoriteProduct);
            }

            await favoriteProductsRepo.Save();
        }

    }
}