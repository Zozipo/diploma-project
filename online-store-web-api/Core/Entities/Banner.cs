using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    [Table("Banners")]
    public class Banner : BaseEntity<int>
    {
        public string Image { get; set; }
    }
}
