using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stone.Entities;

namespace Stone.Persistence.Configurations
{
    public class SaleConfiguration : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> builder)
        {
            builder.Property(x => x.OperationNumber)
                .IsUnicode(false)
                .HasMaxLength(10);

            builder.Property(x => x.SaleDate)
                .HasColumnType("date")
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(x => x.Total)
                .HasColumnType("decimal(10,2)");

            builder.ToTable(nameof(Sale), "Musicales");
        }
    }
}
