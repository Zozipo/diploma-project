using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    [Table("ViewedProducts")]
    public class ViewedProduct : BaseEntity<int>
    {
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public DateTime ViewedAt { get; set; }
    }
}
