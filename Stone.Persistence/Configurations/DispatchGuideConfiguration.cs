using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stone.Entities;

namespace Stone.Persistence.Configurations
{
    public class DispatchGuideConfiguration : IEntityTypeConfiguration<DispatchGuide>
    {
        public void Configure(EntityTypeBuilder<DispatchGuide> builder)
        {
            builder.Property(x => x.Folio).HasMaxLength(20);
            builder.Property(x => x.TaxRate).HasPrecision(10, 3);
            builder.HasQueryFilter(x => x.Active);

            builder.ToTable("guia_despacho");

            builder.Property(x => x.CreateAt)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(GETUTCDATE())");
        }
    }
}
