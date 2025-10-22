using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    [Table("OrderHistory")]
    public class OrderHistory : BaseEntity<int>
    {
        public string UserId { get; set; }
        public User User { get; set; }
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
        public DateTime OrderedAt { get; set; }
    }
}
