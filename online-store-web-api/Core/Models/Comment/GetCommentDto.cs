using Core.Models.Product;
using Core.Models.User;

namespace Core.Models.Comment
{
    public class GetCommentDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int ProductId { get; set; }
        public GetUserDto User { get; set; }
        public List<GetCommnetImageDto>? Images { get; set; }


        public float Rating { get; set; }

    }
}
