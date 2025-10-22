using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    [Table("Orders")]
    public class Order : BaseEntity<int>
    {
        public string UserId { get; set; }
        public User User { get; set; }
        public CreditCard CreditCard { get; set; }
        public int? CreditCardId { get; set; }
        public Address Address { get; set; }
        public int? AddressId { get; set; }
        public string? PostalAddress { get; set; }
        public bool PayUponReceipt { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal DeliveryPrice { get; set; }
        public double TotalPrice { get; set; }
        public string OrderStatus { get; set; }
        public string Recipient { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
