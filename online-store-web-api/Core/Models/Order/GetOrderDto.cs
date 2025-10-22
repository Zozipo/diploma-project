namespace Core.Models.Order
{
    public class GetOrderDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string AdressId { get; set; }
        public string CreditCardId { get; set; }
        public List<GetOrderItemDto> OrderedItems { get; set; }
    }
}
