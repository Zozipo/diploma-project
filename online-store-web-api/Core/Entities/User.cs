using Microsoft.AspNetCore.Identity;

namespace Core.Entities
{
    public class User : IdentityUser
    {
        public bool IsDeleted { get; set; } = false;
        public DateTime DateCreated { get; set; } = DateTime.Now.ToUniversalTime();
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Sex { get; set; }
        public string Position { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public string Image { get; set; }
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public int CartId { get; set; }
        public virtual Cart Cart { get; set; }
        public int? CreditCardId { get; set; }
        public virtual CreditCard CreditCard { get; set; }
        public int? AddressId { get; set; }
        public virtual Address Address { get; set; }
        public int? PromoCodeForNextUseId { get; set; }
        public virtual PromoCode PromoCodeForNextUse { get; set; }
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
        public virtual ICollection<TablePermission> TablePermissions { get; set; } = new List<TablePermission>();
        public virtual ICollection<FavoriteProduct> FavoriteProducts { get; set; } = new List<FavoriteProduct>();
        public virtual ICollection<ViewedProduct> ViewedProducts { get; set; } = new List<ViewedProduct>();
    }
}
