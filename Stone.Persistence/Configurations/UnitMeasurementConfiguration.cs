using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stone.Entities;

namespace Stone.Persistence.Configurations
{
    public class UnitMeasurementConfiguration : IEntityTypeConfiguration<UnitMeasurement>
    {
        public void Configure(EntityTypeBuilder<UnitMeasurement> builder)
        {
            builder.Property(x => x.Description)
                            .HasMaxLength(100)
                            .IsUnicode(false);

            builder.ToTable("tipo_medida");

            builder.Property(x => x.CreateAt)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(GETUTCDATE())");
        }
    }
}
