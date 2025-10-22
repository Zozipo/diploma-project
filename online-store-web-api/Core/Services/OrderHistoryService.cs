using AutoMapper;
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
    public class OrderHistoryService(IRepository<OrderHistory> ordersHistoryRepo,
                                          IRepository<Order> ordersRepo,
                                          UserManager<User> userManager, IMapper mapper) : IOrderHistoryService
    {
        
        
        public async Task<IEnumerable<OrderHistory>> GetAll()
        {
            var ordersHistory = await ordersHistoryRepo.GetAllBySpec(new OrdersHistory.All());
            return ordersHistory;
        }

        public async Task<OrderHistory> GetById(int id)
        {
            var ordersHistory = await ordersHistoryRepo
                                            .GetBySpec(new OrdersHistory.ById(id)) ??
                                        throw new HttpException(ErrorMessages.OrdersHistoryByIdNotFound,
                                            HttpStatusCode.NotFound);

            return ordersHistory;
        }
        public async Task<IEnumerable<GetOrderPreviewDto>> GetOrderHistoryByUserId(string userId)
        {
            _ = await userManager
                    .FindByIdAsync(userId) ??
                throw new HttpException(ErrorMessages.UserByIdNotFound, HttpStatusCode.NotFound);

            var userOrdersHistory = await ordersHistoryRepo
                .GetAllBySpec(new OrdersHistory.ByUserId(userId));

            return mapper.Map<IEnumerable<GetOrderPreviewDto>>(userOrdersHistory.Select(ufp => ufp.Order));
        }

        public async Task Add(string userId, int orderId)
        {
            _ = await userManager
                    .FindByIdAsync(userId) ??
                throw new HttpException(ErrorMessages.UserByIdNotFound, HttpStatusCode.NotFound);
            _ = await ordersRepo
                    .GetBySpec(new Orders.ById(orderId)) ??
                throw new HttpException(ErrorMessages.OrdersHistoryByIdNotFound, HttpStatusCode.NotFound);

            var existingRelationship = await ordersHistoryRepo
                .GetBySpec(new OrdersHistory.ByUserAndOrderIds(userId, orderId));

            if (existingRelationship != null)
            {
                existingRelationship.OrderedAt = DateTime.Now.ToUniversalTime();
                await ordersHistoryRepo.Update(existingRelationship);
            }
            else
            {
                var userOrdersHIstory= await ordersHistoryRepo
                    .GetAllBySpec(new OrdersHistory.ByUserId(userId));

                if (userOrdersHIstory.Count() >= 10)
                {
                    var oldestRelationship = userOrdersHIstory
                        .OrderBy(rvp => rvp.OrderedAt)
                        .First();

                    await ordersHistoryRepo.Delete(oldestRelationship);
                }

                var newRelationship = new OrderHistory
                {
                    UserId = userId,
                    OrderId = orderId,
                    OrderedAt = DateTime.Now.ToUniversalTime()
                };

                await ordersHistoryRepo.Insert(newRelationship);
            }

            await ordersHistoryRepo.Save();
        }

        public async Task Remove(string userId, int orderId)
        {
            _ = await userManager
                    .FindByIdAsync(userId) ??
                throw new HttpException(ErrorMessages.UserByIdNotFound, HttpStatusCode.NotFound);
            _ = await ordersRepo
                    .GetBySpec(new Orders.ById(orderId)) ??
                throw new HttpException(ErrorMessages.OrderByIdNotFound, HttpStatusCode.NotFound);

            var existingRelationship = await ordersHistoryRepo
                                           .GetBySpec(new OrdersHistory.ByUserAndOrderIds(userId,
                                               orderId)) ??
                                       throw new HttpException(ErrorMessages.OrdersHistoryByIdNotFound,
                                           HttpStatusCode.NotFound);

            await ordersHistoryRepo.Delete(existingRelationship.Id);
            await ordersHistoryRepo.Save();
        }

        public async Task Delete(int id)
        {
            _ = await ordersHistoryRepo
                    .GetBySpec(new OrdersHistory.ById(id)) ??
                throw new HttpException(ErrorMessages.OrdersHistoryByIdNotFound,
                    HttpStatusCode.NotFound);

            await ordersHistoryRepo.Delete(id);
            await ordersHistoryRepo.Save();
        }


    }
}