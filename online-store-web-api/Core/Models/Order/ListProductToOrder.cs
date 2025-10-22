namespace Core.Models.Order
{
    public class ListProductToOrder
    {
        public int ProductId { get; set; }
        public int CountProducts { get; set; }
        public int Price { get; set; }
        public int Discount { get; set; }
    }
}
