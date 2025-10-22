namespace Core.Models.Category
{
    public class GetCategoryDto
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int? ParentCategoryId { get; set; }
       
    }
}
