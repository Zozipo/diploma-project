namespace Core.Models.CreditCard
{
    public class GetCreditCardDto
    {
        public int Id { get; set; }
        public string CardHolder { get; set; }
        public string CardNumber { get; set; }
        public string Cvv { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string? UserId { get; set; }
    }
}
