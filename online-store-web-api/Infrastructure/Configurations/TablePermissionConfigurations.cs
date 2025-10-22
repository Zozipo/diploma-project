using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class TablePermissionConfigurations : IEntityTypeConfiguration<TablePermission>
    {
        public void Configure(EntityTypeBuilder<TablePermission> builder)
        {
            builder
                .HasOne(p => p.User)
                .WithMany(u => u.TablePermissions)
                .HasForeignKey(p => p.UserId);

            builder
                .HasOne(tp => tp.ModeratingTable)
                .WithMany(mt => mt.TablePermissions)
                .HasForeignKey(tp => tp.ModeratingTableId);
        }
    }
}
