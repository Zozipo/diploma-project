using Ardalis.Specification;
using Core.Entities;

namespace Core.Specification
{
    public static class FavoriteProducts
    {
        public class All : Specification<FavoriteProduct>
        {
            public All()
            {
                Query
                    .Include(ufp => ufp.User)
                    .Include(ufp => ufp.Product);
            }
        }

        public class ById : Specification<FavoriteProduct>
        {
            public ById(int id)
            {
                Query
                    .Where(ufp => ufp.Id == id)
                    .Include(ufp => ufp.Product)
                    .Include(ufp => ufp.User);
            }
        }

        public class ByUserId : Specification<FavoriteProduct>
        {
            public ByUserId(string userId)
            {
                Query
                    .Where(ufp => ufp.UserId == userId)
                    .Include(ufp => ufp.Product)
                    .ThenInclude(p => p.Images);
            }
        }

        public class ByProductId : Specification<FavoriteProduct>
        {
            public ByProductId(int productId)
            {
                Query
                    .Where(ufp => ufp.ProductId == productId)
                    .Include(ufp => ufp.User);
            }
        }

        public class ByUserAndProductIds : Specification<FavoriteProduct>
        {
            public ByUserAndProductIds(string userId, int productId)
            {
                Query
                    .Where(ufp => ufp.UserId == userId && ufp.ProductId == productId);
            }
        }
    }
}
