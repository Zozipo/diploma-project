using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class CreditCardConfigurations : IEntityTypeConfiguration<CreditCard>
    {
        public void Configure(EntityTypeBuilder<CreditCard> builder)
        {
            builder
                .HasOne(c => c.User)
                .WithOne(c => c.CreditCard)
                .HasForeignKey<CreditCard>(c => c.UserId);
        }
    }
}
