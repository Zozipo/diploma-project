using Ardalis.Specification;
using Core.Entities;

namespace Core.Specification
{
    public static class OrdersHistory
    {
        public class All : Specification<OrderHistory>
        {
            public All()
            {
                Query
                    .Include(rvp => rvp.User)
                    .Include(rvp => rvp.Order);
            }
        }

        public class ById : Specification<OrderHistory>
        {
            public ById(int id)
            {
                Query
                    .Where(rvp => rvp.Id == id)
                    .Include(rvp => rvp.Order)
                    .Include(rvp => rvp.User);
            }
        }

        public class ByUserId : Specification<OrderHistory>
        {
            public ByUserId(string userId)
            {
                Query
                    .Where(ufp => ufp.UserId == userId)
                    .Include(ufp => ufp.Order);
            }
        }

        public class ByUserAndOrderIds : Specification<OrderHistory>
        {
            public ByUserAndOrderIds(string userId, int orderId)
            {
                Query
                    .Where(rvp => rvp.UserId == userId && rvp.OrderId == orderId);
            }
        }
    }
}
