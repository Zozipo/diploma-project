using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    [Table("Categories")]
    public class Category : BaseEntity<int>
    {
        [MaxLength(255), MinLength(3)]
        public string Name { get; set; }
        [MaxLength(4000)]
        public string Description { get; set; }
        public string Image { get; set; }
        public int? ParentCategoryId { get; set; }
        public Category? ParentCategory { get; set; }
        public List<Category> Subcategries { get; set; }
        public List<Product> Products { get; set; }
    }
}
