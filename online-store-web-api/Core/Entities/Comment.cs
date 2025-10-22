using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    [Table("Comments")]
    public class Comment : BaseEntity<int>
    {
        [MaxLength(2000)]
        public string Text { get; set; }
        public int ProductId { get; set; }
        public float Rating { get; set; }
        public string UserId { get; set; }
        public virtual Product Product { get; set; }
        public virtual User User { get; set; }
        public ICollection<CommentImage>? Images { get; set; }

    }
}
