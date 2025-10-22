using Ardalis.Specification;
using Core.Entities;

namespace Core.Specification
{
    public static class Products
    {
        public class OrderedAll : Specification<Product>
        {
            public OrderedAll()
            {
                Query
                    .OrderByDescending(x => x.Title)
                    .Include(x => x.Category)
                    .Include(x => x.Images);
            }
        }

        public class ById : Specification<Product>
        {
            public ById(int id)
            {
                Query
                    .Where(x => x.Id == id)
                    .Include(x => x.Category)
                    .Include(x => x.Images);
            }
        }

        public class ProductsByCategory : Specification<Product>
        {
            public ProductsByCategory(int categoryId)
            {
                Query
                    .Where(x => x.CategoryId == categoryId)
                    .OrderByDescending(x => x.Title)
                    .Include(x => x.Category)
                    .Include(x => x.Images);
            }
        }

        public class PromotionalOffers : Specification<Product>
        {
            public PromotionalOffers()
            {
                Query
                    .Where(x => x.Discount.HasValue && x.Discount > 0)
                    .Include(x => x.Category)
                    .Include(x => x.Images);
            }
        }

        public class ProductsByIds : Specification<Product>
        {
            public ProductsByIds(List<int> productIds)
            {
                Query.Where(p => productIds.Contains(p.Id)).Include(x => x.Images); ;
            }
        }

        public class PopularProducts : Specification<Product>
        {
            public PopularProducts()
            {
                Query
                    .OrderByDescending(x => x.ViewedProducts.Count)
                    .Include(x => x.Category)
                    .Include(x => x.Images);
            }
        }

        public class PopularProductsByCategoryId : Specification<Product>
        {
            public PopularProductsByCategoryId(int categoryId)
            {
                Query
                    .Where(x => x.CategoryId == categoryId)
                    .OrderByDescending(x => x.ViewedProducts.Count)
                    .Include(x => x.Category)
                    .Include(x => x.Images);
            }
        }
    }
}
