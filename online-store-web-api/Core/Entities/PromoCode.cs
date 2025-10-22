using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    [Table("PromoCodes")]
    public class PromoCode : BaseEntity<int>
    {
        public string Code { get; set; }
        [Range(0, 100)]
        public decimal DiscountPercentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? RemainingUses { get; set; }
    }
}
