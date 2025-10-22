using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Models.Product;
using Core.Specification;
using Core.Helpers;
using Core.Resources;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Core.Models;
using static Core.Specification.Orders;
using static Core.Specification.Products;
using Microsoft.Extensions.Logging;


namespace Core.Services
{
    public class ProductsService(IRepository<Product> productsRepo,
                                 ICategoriesService categoriesService,
                                 IRepository<ProductImage> productImagesRepo,
                                 IRepository<OrderItem>orderRepo,
                                 IRepository<FavoriteProduct> favoriteProductsRepo,
                                 IMapper mapper,
                                 IImagesService imagesService,
                                 ILogger<ProductsService> logger) : IProductsService
    {
        public async Task<IEnumerable<GetProductDto>> GetAll()
        {
            var products = await productsRepo.GetAllBySpec(new Products.OrderedAll());
            return mapper.Map<IEnumerable<GetProductDto>>(products);
        }

        public async Task<GetProductDto?> GetById(int id)
        {
            Product product = await productsRepo
                                  .GetBySpec(new Products.ById(id)) ??
                              throw new HttpException(ErrorMessages.ProductByIdNotFound, HttpStatusCode.NotFound);

            return mapper.Map<GetProductDto>(product);
        }

        public async Task Update(int productId, UpdateProductDto product)
        {
            var existingProduct = await productsRepo.GetBySpec(new Products.ById(productId));

            if (existingProduct == null)
                throw new HttpException(ErrorMessages.ProductByIdNotFound, HttpStatusCode.NotFound);

            if (existingProduct.Images != null && existingProduct.Images.Any())
            {
                foreach (var image in existingProduct.Images)
                {
                    imagesService.DeleteImage(image.Image);

                    await productImagesRepo.Delete(image.Id);
                    await productImagesRepo.Save();

                }
            }

            var productEntity = mapper.Map<Product>(product);
            productEntity.Id = productId;

            await productsRepo.Update(mapper.Map<Product>(productEntity));
            await productsRepo.Save();

            if (product.Images != null && product.Images.Any())
            {
                foreach (var imageFile in product.Images)
                {
                    var productImage = new ProductImage
                    {
                        Image = imagesService.SaveImage(imageFile),
                        Product = productEntity
                    };

                    await productImagesRepo.Insert(productImage);
                }
            }

            await productImagesRepo.Save();
        }

        public async Task Create(CreateProductDto product)
        {
            var newProduct = mapper.Map<Product>(product);

            if (product.Images != null && product.Images.Any())
            {
                var imageEntities = new List<ProductImage>();
                foreach (var imageFile in product.Images)
                {
                    var imageEntity = new ProductImage
                    {
                        Image = imagesService.SaveImage(imageFile),
                        Product = newProduct
                    };
                    imageEntities.Add(imageEntity);
                }

                newProduct.Images = imageEntities;

                await productsRepo.Insert(newProduct);
                await productsRepo.Save();
            }
        }

        public async Task Delete(int id)
        {
            var existingProduct = await productsRepo.GetByID(id) ??
                                  throw new HttpException(ErrorMessages.ProductByIdNotFound, HttpStatusCode.NotFound);

            if (existingProduct.Images != null && existingProduct.Images.Any())
            {
                foreach (var image in existingProduct.Images)
                {
                    imagesService.DeleteImage(image.Image);

                    await productImagesRepo.Delete(image.Id);
                    await productImagesRepo.Save();

                }
            }

            await productsRepo.Delete(id);
            await productsRepo.Save();
        }

        public async Task<IEnumerable<GetProductPreviewDto>> GetFrequentlyBoughtTogether(int productId)
        {
                var frequentlyBoughtTogetherSpecification = new FrequentlyBoughtTogether(productId);
                var orderItems = await orderRepo.GetAllBySpec(frequentlyBoughtTogetherSpecification);
                var relatedProductIds = orderItems.Select(oi => oi.ProductId).Distinct().ToList();
                var productsEnumerable = await productsRepo.GetAllBySpec(new ProductsByIds(relatedProductIds));

                var frequentlyBoughtTogetherProducts = productsEnumerable.ToList();

                logger.LogInformation($"Found {frequentlyBoughtTogetherProducts.Count} frequently bought together products.");

                return mapper.Map<IEnumerable<GetProductPreviewDto>>(frequentlyBoughtTogetherProducts);
        }

        public async Task<PaginatedList<GetProductDto>> GetByFilter(ItemsFilter filter)
        {
            var query = await productsRepo.GetAllBySpecQueryable(new Products.OrderedAll());

            if (!string.IsNullOrEmpty(filter.SearchTerm))
            {
                var searchTermLower = filter.SearchTerm.ToLower();
                query = query.Where(u => u.Title.ToLower().Contains(searchTermLower));
            }
            if (!string.IsNullOrEmpty(filter.Sorting))
            {
                var searchTermLower = filter.Sorting.ToLower();
                query = query.Where(u => u.CategoryId == Convert.ToInt32(searchTermLower));
            }

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalItems / filter.PageSize);

            var sortingMap = new Dictionary<string, Func<IQueryable<Product>, IOrderedQueryable<Product>>>
            {
                { "Id", q => filter.SortDirection == "asc" ? q.OrderBy(product => product.Id) : q.OrderByDescending(product => product.Id) },
                { "Name", q => filter.SortDirection == "asc" ? q.OrderBy(product => product.Title) : q.OrderByDescending(product => product.Title) },
                { "Price", q => filter.SortDirection == "asc" ? q.OrderBy(product => product.Price) : q.OrderByDescending(product => product.Price) },
                { "Discount", q => filter.SortDirection == "asc" ? q.OrderBy(product => product.Discount) : q.OrderByDescending(product => product.Discount) },
                { "Rating", q => filter.SortDirection == "asc" ? q.OrderBy(product => product.Rating) : q.OrderByDescending(product => product.Rating) },
            };

            if (sortingMap.TryGetValue(filter.SortBy, out var sortingFunction))
                query = sortingFunction(query);

            var skipAmount = (filter.PageNumber - 1) * filter.PageSize;
            query = query.Skip(skipAmount).Take(filter.PageSize);

            var products = await query.ToListAsync();

            var productsMapped = products.Select(mapper.Map<GetProductDto>).ToList();

            PaginatedList<GetProductDto> productPaginatedList = new()
            {
                TotalPages = totalPages,
                Items = productsMapped,
            };

            return productPaginatedList;
        }

        public async Task<IEnumerable<GetProductPreviewDto>> GetProductsByCategory(int categoryId)
        {
            var productsInCategory = await productsRepo.GetAllBySpec(new Products.ProductsByCategory(categoryId));

            return mapper.Map<IEnumerable<GetProductPreviewDto>>(productsInCategory);
        }

        public async Task<IEnumerable<GetProductPreviewDto>> GetPromotionalOffers(int count)
        {
            var promotionalOffers = await productsRepo.GetAllBySpec(new Products.PromotionalOffers());

            promotionalOffers = promotionalOffers.Take(count);

            return mapper.Map<IEnumerable<GetProductPreviewDto>>(promotionalOffers);
        }

        public async Task<IEnumerable<GetProductPreviewDto>> GetPromotionalOffersWithFavorites(int count, string userId)
        {
            var promotionalOffers = await productsRepo.GetAllBySpec(new Products.PromotionalOffers());
            promotionalOffers = promotionalOffers.Take(count);

            var promotionalOffersDto = mapper.Map<IEnumerable<GetProductPreviewDto>>(promotionalOffers).ToList();

            var userFavoriteProducts = await favoriteProductsRepo.GetAllBySpec(new FavoriteProducts.ByUserId(userId));

            foreach (var offerDto in promotionalOffersDto)
            {
                offerDto.IsFavorite = userFavoriteProducts.Any(f => f.ProductId == offerDto.Id);
            }

            return promotionalOffersDto;
        }

        public async Task<IEnumerable<GetProductPreviewDto>> GetPopularProducts(int count)
        {
            var popularProducts = await productsRepo.GetAllBySpec(new Products.PopularProducts());

            popularProducts = popularProducts.Take(count);

            return mapper.Map<IEnumerable<GetProductPreviewDto>>(popularProducts);
        }

        public async Task<IEnumerable<GetProductPreviewDto>> GetPopularProductsWithFavorites(int count, string userId)
        {
            var popularProducts = await productsRepo.GetAllBySpec(new Products.PopularProducts());
            popularProducts = popularProducts.Take(count);

            var popularProductsDto = mapper.Map<IEnumerable<GetProductPreviewDto>>(popularProducts);

            var userFavoriteProducts = await favoriteProductsRepo.GetAllBySpec(new FavoriteProducts.ByUserId(userId));

            foreach (var productDto in popularProductsDto)
            {
                productDto.IsFavorite = userFavoriteProducts.Any(f => f.ProductId == productDto.Id);
            }

            return popularProductsDto;
        }

        public async Task<IEnumerable<GetProductPreviewDto>> GetPopularProductsByUserId(string userId, int count)
        {
            var mostWatchedCategory = await categoriesService.GetMostWatchedCategoryByUserId(userId);

            if (mostWatchedCategory == null)
            {
                var allCategories = await categoriesService.GetAll();
                var random = new Random();
                var randomIndex = random.Next(0, allCategories.Count());
                mostWatchedCategory = allCategories.ElementAt(randomIndex);
            }

            var popularProducts = await GetPopularProductsByCategory(mostWatchedCategory.Id, count);

            var userFavoriteProducts = await favoriteProductsRepo.GetAllBySpec(new FavoriteProducts.ByUserId(userId));

            foreach (var popularProduct in popularProducts)
            {
                popularProduct.IsFavorite = userFavoriteProducts.Any(f => f.ProductId == popularProduct.Id);
            }

            return mapper.Map<IEnumerable<GetProductPreviewDto>>(popularProducts);
        }

        public async Task<IEnumerable<GetProductPreviewDto>> GetPopularProductsByCategory(int categoryId, int count)
        {
            var popularProducts = await productsRepo.GetAllBySpec(new Products.PopularProductsByCategoryId(categoryId));

            popularProducts = popularProducts.Take(count);

            return mapper.Map<IEnumerable<GetProductPreviewDto>>(popularProducts);
        }
    }
}
