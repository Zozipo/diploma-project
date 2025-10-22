using AutoMapper;
using Core.Constants;
using Core.Entities;
using Core.Helpers;
using Core.Interfaces;
using Core.Models.Order;
using Core.Resources;
using Core.Specification;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace Core.Services
{
    public class OrdersService(UserManager<User> userManager,
                              IRepository<Order> ordersRepo,
                              IRepository<Cart> cartsRepo,
                              IMapper mapper) : IOrdersService
    {

        public async Task<IEnumerable<GetOrderDto>> GetAll()
        {
                var orders = await ordersRepo.GetAllBySpec(new Orders.All());

            return mapper.Map<IEnumerable<GetOrderDto>>(orders);
        }

        public async Task<GetOrderDto> GetById(int id)
        {
            Order order = await ordersRepo.GetBySpec(new Orders.ById(id))
                ?? throw new HttpException(ErrorMessages.OrderByIdNotFound, HttpStatusCode.NotFound);

            return mapper.Map<GetOrderDto>(order);
        }

        public async Task<IEnumerable<GetOrderDto>> GetByUserId(string userId)
        {
            var orders = await ordersRepo.GetBySpec(new Orders.ByUserId(userId))
                ?? throw new HttpException(ErrorMessages.OrderByIdNotFound, HttpStatusCode.NotFound);
            return mapper.Map<IEnumerable<GetOrderDto>>(orders);
        }

        public async Task Create(CreateOrderDto createOrderDto)
        {
            var user = await userManager.FindByIdAsync(createOrderDto.UserId)
                       ?? throw new HttpException(ErrorMessages.UserByIdNotFound, HttpStatusCode.NotFound);

            var cart = await cartsRepo
                           .GetBySpec(new Carts.ByUserId(user.Id))
                       ?? throw new HttpException(ErrorMessages.CartByIdNotFound, HttpStatusCode.NotFound);

            if (cart.CartItems.Count == 0)
            {
                throw new HttpException(ErrorMessages.CartIsEmpty, HttpStatusCode.BadRequest);
            }

            var order = mapper.Map<Order>(createOrderDto);

            order.OrderItems = mapper.Map<List<OrderItem>>(cart.CartItems);

            cart.CartItems.Clear();

            await cartsRepo.Update(cart);
            await cartsRepo.Save();

            order.OrderStatus = OrderStatus.ProcessDelivery;

            await ordersRepo.Insert(order);
            await ordersRepo.Save();
        }

        public async Task ChangeStatus(int id) 
        {
            Order order = await ordersRepo.GetBySpec(new Orders.ById(id))
                ?? throw new HttpException(ErrorMessages.OrderByIdNotFound, HttpStatusCode.NotFound);

            order.OrderStatus = OrderStatus.Delivered;

            await ordersRepo.Update(order);
            await ordersRepo.Save();
        }
    }
}
