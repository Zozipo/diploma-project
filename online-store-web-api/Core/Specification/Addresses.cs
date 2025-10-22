using Ardalis.Specification;
using Core.Entities;

namespace Core.Specification
{
    public class Addresses
    {
        public class ById : Specification<Address>
        {
            public ById(int id)
            {
                Query
                    .Where(x => x.Id == id)
                    .Include(x => x.User);
            }
        }

        public class ByUserId : Specification<Address>
        {
            public ByUserId(string userId)
            {
                Query
                    .Where(x => x.UserId == userId);
            }
        }

        public class All : Specification<Address>
        {
            public All()
            {
                Query
                    .Include(x => x.User);
            }
        }
    }
}
