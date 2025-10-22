using Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Configurations
{
    public class ProductConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.Id);

            builder
                .Property(x => x.Title)
                .HasMaxLength(200)
                .IsRequired();

            builder
                .HasOne(x => x.Category)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.CategoryId);

            builder
                .HasMany(a => a.Images)
                .WithOne(i => i.Product)
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(p => p.FavoriteProducts)
                .WithOne(ufp => ufp.Product)
                .HasForeignKey(ufp => ufp.ProductId);

            builder
                .HasMany(p => p.ViewedProducts)
                .WithOne(rvp => rvp.Product)
                .HasForeignKey(rvp => rvp.ProductId);
        }
    }
}
