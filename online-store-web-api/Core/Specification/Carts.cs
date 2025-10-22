using Ardalis.Specification;
using Core.Entities;

namespace Core.Specification
{
    public static class Carts
    {
        public class ById : Specification<Cart>
        {
            public ById(int id)
            {
                Query
                    .Where(c => c.Id == id)
                    .Include(c => c.User)
                    .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                    .ThenInclude(p => p.Images);
            }
        }

        public class ByUserId : Specification<Cart>
        {
            public ByUserId(string userId)
            {
                Query
                    .Where(c => c.UserId == userId)
                    .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                    .ThenInclude(p => p.Images);
            }
        }

        public class All : Specification<Cart>
        {
            public All()
            {
                Query
                    .Include(c => c.User)
                    .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                    .ThenInclude(p => p.Images);
            }
        }
    }
}
