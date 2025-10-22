using Core.Models.Product;

namespace Core.Models.Cart
{
    public class GetCartItemDto
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public int Quantity { get; set; }
        public GetProductPreviewDto Product { get; set; }
    }
}
