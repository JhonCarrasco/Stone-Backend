using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stone.Entities;

namespace Stone.Persistence.Configurations
{
    public class BudgetConfiguration : IEntityTypeConfiguration<Budget>
    {
        public void Configure(EntityTypeBuilder<Budget> builder)
        {
            builder.Property(x => x.ProjectName).HasMaxLength(100);
            builder.Property(x => x.Address).HasMaxLength(100);
            builder.Property(x => x.Description).HasMaxLength(200);
            builder.Property(x => x.Material).HasMaxLength(100);
            builder.HasQueryFilter(x => x.Active);
            //builder.Property(x => x.Iva).HasComputedColumnSql("(Iva / 100)"); // Assuming Iva is a percentage, this will compute the tax amount based on the SubTotal.

            builder.ToTable("presupuesto");

            builder.Property(x => x.CreateAt)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(GETUTCDATE())");

          
        }
    }
}
