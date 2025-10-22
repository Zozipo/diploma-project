namespace Core.Models
{
    public class ItemsFilter
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SearchTerm { get; set; } = "";
        public string SortBy { get; set; } = "Name";
        public string SortDirection { get; set; } = "desc";
        public string Sorting { get; set; } = "";
    }
}
