using Ardalis.Specification;
using Core.Entities;

namespace Core.Specification
{
    public static class Categories
    {
        public class ByName : Specification<Category>
        {
            public ByName(string name)
            {
                Query
                    .Where(x => x.Name == name)
                    .Include(x => x.ParentCategory);
            }
        }

        public class ById : Specification<Category>
        {
            public ById(int id)
            {
                Query
                    .Where(x => x.Id == id)
                    .Include(x => x.ParentCategory);
            }

            public ById(int? id)
            {
                Query
                    .Where(x => x.Id == id)
                    .Include(x => x.ParentCategory);
            }
        }

        public class All : Specification<Category>
        {
            public All()
            {
                Query
                    .Include(x => x.ParentCategory);
            }
        }
      

        public class ByParentCategoryId : Specification<Category>
        {
            public ByParentCategoryId(int? parentCategoryId)
            {
                Query
                    .Where(x => x.ParentCategoryId == parentCategoryId)
                    .Include(x => x.ParentCategory);
            }
        }

        public class ByParentCategoryName : Specification<Category>
        {
            public ByParentCategoryName(string parentCategoryName)
            {
                Query
                    .Include(x => x.ParentCategory)
                    .Where(x => x.ParentCategory.Name == parentCategoryName);
            }
        }
    }
}
