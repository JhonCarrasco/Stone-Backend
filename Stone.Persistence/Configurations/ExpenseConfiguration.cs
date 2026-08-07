using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stone.Entities;

namespace Stone.Persistence.Configurations
{
    public class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
    {
        public void Configure(EntityTypeBuilder<Expense> builder)
        {
            builder.Property(x => x.Description).HasMaxLength(400);
            builder.HasQueryFilter(x => x.Active);

            builder.ToTable("rendir_gastos");

            builder.Property(x => x.CreateAt)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(GETUTCDATE())");
        }
    }
}
