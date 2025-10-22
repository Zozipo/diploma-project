namespace Core.Models.Category
{
    public class GetCategorySelectionDto
    {
        public ICollection<GetCategoryDto> ParentCategories { get; set; }
        public ICollection<GetCategoryDto> Categories { get; set; }
    }
}