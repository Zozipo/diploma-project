namespace Core.Models.Promocode
{
    public class CreatePromoCodeDto
    {
        public string Code { get; set; }
        public decimal DiscountPercentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? RemainingUses { get; set; }
    }
}
