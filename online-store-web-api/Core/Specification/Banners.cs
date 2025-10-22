using Ardalis.Specification;
using Core.Entities;

namespace Core.Specification
{
    public static class Banners
    {
        public class ById : Specification<Banner>
        {
            public ById(int id)
            {
                Query
                    .Where(c => c.Id == id);
            }
        }
    }
}
