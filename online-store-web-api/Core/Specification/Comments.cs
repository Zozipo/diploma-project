using Ardalis.Specification;
using Core.Entities;

namespace Core.Specification
{
    public static class Comments
    {
        public class ById : Specification<Comment>
        {
            public ById(int id)
            {
                Query
                    .Where(x => x.Id == id)
                    .Include(x => x.User)
                    .Include(x => x.Images);
            }
        }

        public class ByProductId : Specification<Comment>
        {
            public ByProductId(int productId)
            {
                Query
                    .Where(x => x.ProductId == productId)
                    .Include(x => x.User)
                    .Include(x => x.Images);
            }
        }

        public class ByUserId : Specification<Comment>
        {
            public ByUserId(string userId)
            {
                Query
                    .Where(x => x.UserId == userId)
                    .Include(x => x.Images);
            }
        }

        public class All : Specification<Comment>
        {
            public All()
            {
                Query
                    .Include(x => x.User)
                .Include(x => x.Images);
            }
        }
    }
}
