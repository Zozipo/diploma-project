using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    [Table("ProductImages")]
    public class ProductImage
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
