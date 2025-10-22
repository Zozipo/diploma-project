using Ardalis.Specification;
using Core.Entities;

namespace Core.Specification
{
    public static class Permissions
    {
        public class All : Specification<TablePermission>
        {
            public All()
            {
                Query
                    .OrderBy(p => p.Id);
            }
        }

        public class ByUserId : Specification<TablePermission>
        {
            public ByUserId(string userId)
            {
                Query
                    .Where(mp => mp.UserId == userId)
                    .Include(mp => mp.ModeratingTable);
            }
        }

        public class ById : Specification<TablePermission>
        {
            public ById(int id)
            {
                Query
                    .Where(mp => mp.Id == id);
            }
        }
    }
}