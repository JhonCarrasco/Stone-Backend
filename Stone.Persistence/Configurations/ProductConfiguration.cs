using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stone.Entities;

namespace Stone.Persistence.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(x => x.Description).HasMaxLength(200);
            builder.Property(x => x.Color).HasMaxLength(100);
            builder.Property(x => x.UnitMeasurement).HasMaxLength(100);
            builder.Property(x => x.Long).HasPrecision(10, 3);
            builder.Property(x => x.Width).HasPrecision(10, 3);
            builder.Property(x => x.Thickness).HasPrecision(10, 3);
            builder.HasQueryFilter(x => x.Active);

            builder.ToTable("producto");

            builder.Property(x => x.CreateAt)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(GETUTCDATE())");
        }
    }
}
