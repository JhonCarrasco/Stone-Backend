using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stone.Entities;

namespace Stone.Persistence.Configurations
{
    public class MaterialVoucherConfiguration : IEntityTypeConfiguration<MaterialVoucher>
    {
        public void Configure(EntityTypeBuilder<MaterialVoucher> builder)
        {
            builder.Property(x => x.SupplierTo).HasMaxLength(100);
            builder.Property(x => x.SupplierTo).HasMaxLength(200);
            builder.HasQueryFilter(x => x.Active);

            builder.ToTable("cupon_material");

            builder.Property(x => x.CreateAt)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(GETUTCDATE())");
        }
    }
}
