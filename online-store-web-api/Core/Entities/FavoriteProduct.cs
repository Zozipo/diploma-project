using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    [Table("FavoriteProducts")]
    public class FavoriteProduct : BaseEntity<int>
    {
        public string UserId { get; set; }
        public User User { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
