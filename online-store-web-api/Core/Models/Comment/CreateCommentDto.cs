using Microsoft.AspNetCore.Http;

namespace Core.Models.Comment
{
    public class CreateCommentDto
    {
        public string Text { get; set; }
        public int ProductId { get; set; }
        public string UserId { get; set; }

        public List<IFormFile>? Images { get; set; }


        public float Rating {  get; set; }
    }
}
