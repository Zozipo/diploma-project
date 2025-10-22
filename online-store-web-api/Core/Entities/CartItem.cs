using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    [Table("CartItems")]
    public class CartItem : BaseEntity<int>
    {
        public int Quantity { get; set; }
        public int CartId { get; set; }
        public Cart Cart { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
