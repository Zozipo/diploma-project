namespace Core.Models
{
    public class PaginatedList<T>
    {
        public int TotalPages { get; set; }
        public ICollection<T> Items { get; set; }
    }
}
