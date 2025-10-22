using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    [Table("Carts")]
    public class Cart : BaseEntity<int>
    {
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
