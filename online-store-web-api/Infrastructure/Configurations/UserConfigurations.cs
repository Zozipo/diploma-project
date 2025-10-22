using Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Configurations
{
    public class UserConfigurations : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .HasMany(u => u.UserRoles)
                .WithOne(ur => ur.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            builder
                .HasOne(u => u.Cart)
                .WithOne(c => c.User)
                .HasForeignKey<Cart>(c => c.UserId);
                
            builder
                .HasMany(u => u.FavoriteProducts)
                .WithOne(ufp => ufp.User)
                .HasForeignKey(ufp => ufp.UserId);

            builder
                .HasMany(u => u.Orders)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.UserId);
                
            builder
                .HasMany(u => u.ViewedProducts)
                .WithOne(rvp => rvp.User)
                .HasForeignKey(rvp => rvp.UserId);

            builder
                .HasOne(c => c.CreditCard)
                .WithOne(c => c.User)
                .HasForeignKey<CreditCard>(c => c.UserId);

            builder
                .HasOne(c => c.Address)
                .WithOne(c => c.User)
                .HasForeignKey<Address>(c => c.UserId);

            builder
                .HasMany(u => u.TablePermissions)
                .WithOne(tp => tp.User)
                .HasForeignKey(tp => tp.UserId);
        }
    }
}
