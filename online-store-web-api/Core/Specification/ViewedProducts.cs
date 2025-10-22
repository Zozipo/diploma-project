using Ardalis.Specification;
using Core.Entities;

namespace Core.Specification
{
    public static class ViewedProducts
    {
        public class All : Specification<ViewedProduct>
        {
            public All()
            {
                Query
                    .Include(rvp => rvp.User)
                    .Include(rvp => rvp.Product);
            }
        }

        public class ById : Specification<ViewedProduct>
        {
            public ById(int id)
            {
                Query
                    .Where(rvp => rvp.Id == id)
                    .Include(rvp => rvp.Product)
                    .Include(rvp => rvp.User);
            }
        }

        public class ByUserId : Specification<ViewedProduct>
        {
            public ByUserId(string userId)
            {
                Query
                    .Where(ufp => ufp.UserId == userId)
                    .OrderByDescending(vp => vp.ViewedAt)
                    .Include(ufp => ufp.Product)
                    .ThenInclude(p => p.Images);
            }
        }

        public class ByUserAndProductIds : Specification<ViewedProduct>
        {
            public ByUserAndProductIds(string userId, int productId)
            {
                Query
                    .Where(rvp => rvp.UserId == userId && rvp.ProductId == productId)
                    .OrderByDescending(vp => vp.ViewedAt);
            }
        }

        public class ByUserIdAndDate : Specification<ViewedProduct>
        {
            public ByUserIdAndDate(string userId, DateTime fromDate)
            {
                Query
                    .Where(vp => vp.UserId == userId && vp.ViewedAt >= fromDate)
                    .OrderByDescending(vp => vp.ViewedAt)
                    .Include(vp => vp.Product.Category);
            }
        }
    }
}
