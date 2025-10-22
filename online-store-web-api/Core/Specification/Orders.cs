using Ardalis.Specification;
using Core.Entities;

namespace Core.Specification
{
    public static class Orders
    {
        public class ById : Specification<Order>
        {
            public ById(int id)
            {
                Query
                    .Where(c => c.Id == id)
                    .Include(c => c.User)
                    .Include(c => c.OrderItems)
                    .ThenInclude(ci => ci.Product)
                    .ThenInclude(p => p.Images);
            }
        }
        public class ByUserId : Specification<Order>
        {
            public ByUserId(string userId)
            {
                Query
                    .Where(c => c.UserId == userId)
                    .Include(c => c.OrderItems)
                    .ThenInclude(c => c.Product)
                    .ThenInclude(p => p.Images);
            }
        }
        public class All : Specification<Order>
        {
            public All()
            {
                Query
                    .Include(c => c.User)
                    .Include(c => c.OrderItems)
                    .ThenInclude(c => c.Product);
            }
        }
        public class FrequentlyBoughtTogether : Specification<OrderItem>
        {
            public FrequentlyBoughtTogether(int productId)
            {
                Query
                    .Where(oi => oi.Order.OrderItems.Any(o => o.ProductId == productId))
                    .Include(oi => oi.Order)
                        .ThenInclude(o => o.OrderItems)
                            .ThenInclude(oi => oi.Product)
                    .AsTracking();
            }
        }



    }
}
