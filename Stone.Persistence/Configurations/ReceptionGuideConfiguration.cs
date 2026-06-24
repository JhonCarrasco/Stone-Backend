using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stone.Entities;

namespace Stone.Persistence.Configurations
{
    public class ReceptionGuideConfiguration : IEntityTypeConfiguration<ReceptionGuide>
    {
        public void Configure(EntityTypeBuilder<ReceptionGuide> builder)
        {
            builder.Property(x => x.Folio).HasMaxLength(20);
            builder.Property(x => x.TaxRate).HasPrecision(10, 3);
            builder.HasQueryFilter(x => x.Active);

            builder.ToTable("guia_recepcion");

            builder.Property(x => x.CreateAt)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(GETUTCDATE())");
        }
    }
}
