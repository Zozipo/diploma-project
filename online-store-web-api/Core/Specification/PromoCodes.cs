using Ardalis.Specification;
using Core.Entities;

namespace Core.Specification
{
    public static class PromoCodes
    {
        public class All : Specification<PromoCode>
        {
            public All()
            {
                Query
                    .OrderBy(x => x.Id);
            }
        }

        public class ById : Specification<PromoCode>
        {
            public ById(int id)
            {
                Query
                    .Where(x => x.Id == id);
            }
        }

        public class ByCode : Specification<PromoCode>
        {
            public ByCode(string code)
            {
                Query
                    .Where(x => x.Code == code);
            }
        }
    }
}
