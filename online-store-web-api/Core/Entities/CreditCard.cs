using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    [Table("CreditCards")]
    public class CreditCard : BaseEntity<int>
    {
        public string CardHolder { get; set; }
        public string CardNumber { get; set; }
        public string Cvv { get; set; }
        public DateTime ExpiryDate { get; set; }
        public User User { get; set; }
        public string? UserId { get; set; }
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
