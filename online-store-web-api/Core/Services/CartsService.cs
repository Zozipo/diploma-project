using AutoMapper;
using Core.Entities;
using Core.Helpers;
using Core.Interfaces;
using Core.Models.Cart;
using Core.Resources;
using Core.Specification;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace Core.Services
{
    public class CartsService(UserManager<User> userManager,
                              IRepository<Cart> cartsRepo,
                              IRepository<CartItem> cartItemsRepo,
                              IRepository<Product> productsRepo,
                              IMapper mapper) : ICartsService
    {
        public async Task<IEnumerable<GetCartDto>> GetAll()
        {
            var baskets = await cartsRepo
                .GetAllBySpec(new Carts.All());

            return mapper.Map<IEnumerable<GetCartDto>>(baskets);
        }

        public async Task<GetCartDto> GetById(int id)
        {
            Cart cart = await cartsRepo
                            .GetBySpec(new Carts.ById(id))
                        ?? throw new HttpException(ErrorMessages.CartByIdNotFound, HttpStatusCode.NotFound);

            return mapper.Map<GetCartDto>(cart);
        }

        public async Task<GetCartDto> GetByUserId(string userId)
        {
            var cart = await cartsRepo
                           .GetBySpec(new Carts.ByUserId(userId))
                       ?? throw new HttpException(ErrorMessages.CartByIdNotFound, HttpStatusCode.NotFound);

            return mapper.Map<GetCartDto>(cart);
        }

        public async Task<IEnumerable<GetCartItemDto>> GetCartItemsByUserId(string userId)
        {
            var cart = await cartsRepo
                           .GetBySpec(new Carts.ByUserId(userId))
                       ?? throw new HttpException(ErrorMessages.CartByIdNotFound, HttpStatusCode.NotFound);

            var cartItems = mapper.Map<IEnumerable<GetCartItemDto>>(cart.CartItems);

            return cartItems;
        }

        public async Task Create(CreateCartDto createCartDto)
        {
            _ = await userManager
                    .FindByIdAsync(createCartDto.UserId) ??
                throw new HttpException(ErrorMessages.UserByIdNotFound, HttpStatusCode.NotFound);

            var cart = await cartsRepo.GetBySpec(new Carts.ByUserId(createCartDto.UserId));

            if (cart != null)
                throw new HttpException(ErrorMessages.CartAalreadyExists, HttpStatusCode.NotFound);

            var cartEntity = mapper.Map<Cart>(createCartDto);

            await cartsRepo.Insert(cartEntity);
            await cartsRepo.Save();
        }

        public async Task Delete(int id)
        {
            _ = await cartsRepo
                    .GetByID(id)
                ?? throw new HttpException(ErrorMessages.CartByIdNotFound, HttpStatusCode.NotFound);

            await cartsRepo.Delete(id);
            await cartsRepo.Save();
        }

        public async Task DeleteProduct(int cartId, int productId)
        {
            var cart = await cartsRepo.GetBySpec(new Carts.ById(cartId))
                       ?? throw new HttpException(ErrorMessages.CartByIdNotFound, HttpStatusCode.NotFound);

            _ = await productsRepo.GetBySpec(new Products.ById(productId))
                ?? throw new HttpException(ErrorMessages.ProductByIdNotFound, HttpStatusCode.NotFound);

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);

            if (cartItem != null)
            {
                await cartItemsRepo.Delete(cartItem.Id);
                await cartItemsRepo.Save();
            }
            else
            {
                throw new HttpException(ErrorMessages.CartItemNotFoundInCart, HttpStatusCode.NotFound);
            }
        }

        public async Task AddProduct(int productId, int cartId)
        {
            var cart = await cartsRepo.GetBySpec(new Carts.ById(cartId))
                       ?? throw new HttpException(ErrorMessages.CartByIdNotFound, HttpStatusCode.NotFound);

            _ = await productsRepo.GetBySpec(new Products.ById(productId))
                ?? throw new HttpException(ErrorMessages.ProductByIdNotFound, HttpStatusCode.NotFound);

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);

            if (cartItem != null)
            {
                cartItem.Quantity += 1;

                await cartItemsRepo.Update(cartItem);
                await cartItemsRepo.Save();
            }
            else
            {
                var newCartItem = new CartItem
                {
                    ProductId = productId,
                    CartId = cartId,
                    DateCreated = DateTime.Now.ToUniversalTime(),
                    Quantity = 1
                };

                cart.CartItems.Add(newCartItem);

                await cartsRepo.Update(cart);
                await cartsRepo.Save();
            }
        }

        public async Task RemoveProduct(int productId, int cartId)
        {
            var cart = await cartsRepo.GetBySpec(new Carts.ById(cartId))
                       ?? throw new HttpException(ErrorMessages.CartByIdNotFound, HttpStatusCode.NotFound);

            _ = await productsRepo.GetBySpec(new Products.ById(productId))
                ?? throw new HttpException(ErrorMessages.ProductByIdNotFound, HttpStatusCode.NotFound);

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);

            if (cartItem != null)
            {
                if (cartItem.Quantity > 1)
                {
                    cartItem.Quantity -= 1;

                    await cartItemsRepo.Update(cartItem);
                    await cartItemsRepo.Save();
                }
                else
                {
                    await cartItemsRepo.Delete(cartItem.Id);
                    await cartItemsRepo.Save();
                }
            }
            else
            {
                throw new HttpException(ErrorMessages.CartItemNotFoundInCart, HttpStatusCode.NotFound);
            }
        }
    }
}
