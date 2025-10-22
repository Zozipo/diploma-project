using Core.Models.Category;

namespace Core.Models.Product
{
    public class GetProductPreviewDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public decimal? Discount { get; set; }
        public GetCategoryDto? Category { get; set; }
        public bool IsStock { get; set; }
        public bool? IsFavorite { get; set; }
    }
}