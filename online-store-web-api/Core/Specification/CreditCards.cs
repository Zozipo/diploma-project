using Ardalis.Specification;
using Core.Entities;

namespace Core.Specification
{
    public class CreditCards
    {
        public class ById : Specification<CreditCard>
        {
            public ById(int id)
            {
                Query
                    .Where(x => x.Id == id)
                    .Include(x => x.User);
            }
        }

        public class ByUserId : Specification<CreditCard>
        {
            public ByUserId(string userId)
            {
                Query
                    .Where(x => x.UserId == userId);
            }
        }

        public class All : Specification<CreditCard>
        {
            public All()
            {
                Query
                    .Include(x => x.User);
            }
        }
    }
}
