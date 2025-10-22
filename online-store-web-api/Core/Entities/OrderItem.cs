using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    [Table("OrdersItems")]
    public class OrderItem : BaseEntity<int>
    {
        public int Quantity { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
