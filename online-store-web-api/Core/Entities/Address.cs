using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    [Table("Addresses")]
    public class Address : BaseEntity<int>
    {
        public string Street { get; set; }
        public int HomeNumber { get; set; }
        public int ApartmentNumber { get; set; }
        public int FlatNumber { get; set; }
        public int ZipCode { get; set; }
        public bool IsThereElevator { get; set; }
        public User User { get; set; }
        public string? UserId { get; set; }
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
