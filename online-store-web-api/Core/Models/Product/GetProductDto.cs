namespace Core.Models.Product
{
    public class GetProductDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public ICollection<GetProductImageDto>? Images { get; set; }
        public string DeliveryKit {  get; set; }
        public float Rating { get; set; }
        public int CategoryId { get; set; }
        public bool isStock {  get; set; }
        public decimal? Discount { get; set; }
    }
}
