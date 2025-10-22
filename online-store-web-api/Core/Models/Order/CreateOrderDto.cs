namespace Core.Models.Order
{
    public class CreateOrderDto
    {
        public string UserId { get; set; }
        public int? AddressId { get; set; }
        public int? CreditCardId { get; set; }
        public string? PostalAddress { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal DeliveryPrice { get; set; }
        public double TotalPrice { get; set; }
        public string OrderStatus { get; set; }
        public string Recipient { get; set; }
    }
}
