using Microsoft.AspNetCore.Http;

namespace Core.Models.Product
{
    public class UpdateProductDto
    {
        public string? Title { get; set; }
        public decimal? Price { get; set; }
        public string? Description { get; set; }
        public ICollection<IFormFile>? Images { get; set; }
        public float? Rating { get; set; }
        public string? DeliveryKit { get; set; }
        public int? CategoryId { get; set; }
        public decimal? Discount { get; set; }
        public bool? isStock { get; set; }
    }
}
