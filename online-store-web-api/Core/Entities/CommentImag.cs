using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    [Table("CommentImage")]
    public class CommentImage
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public int CommentId { get; set; }
        public Comment Comment { get; set; }
    }
}
