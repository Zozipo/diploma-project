using Core.Models.Product;

namespace Core.Models.Order
{
    public class GetOrderItemDto
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public GetProductDto Product { get; set; }
    }
}
