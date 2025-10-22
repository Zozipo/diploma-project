namespace Core.Models.Cart
{
    public class GetCartDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public List<GetCartItemDto> CartItems { get; set; }
    }
}
