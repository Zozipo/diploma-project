using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    [Table("Products")]
    public class Product : BaseEntity<int>
    {
        public string? Title { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public string DeliveryKit {  get; set; }
        public ICollection<ProductImage>? Images { get; set; }
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public float Rating { get; set; }
        public int CategoryId { get; set; }
        public bool isStock { get; set; } 
        public Category? Category { get; set; }
        public virtual ICollection<FavoriteProduct> FavoriteProducts { get; set; } = new List<FavoriteProduct>();
        public virtual ICollection<ViewedProduct> ViewedProducts { get; set; } = new List<ViewedProduct>();
        public decimal? Discount { get; set; }

    }
}
