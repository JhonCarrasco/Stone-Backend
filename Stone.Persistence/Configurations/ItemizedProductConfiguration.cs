using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stone.Entities.Budget;

namespace Stone.Persistence.Configurations
{
    public class ItemizedProductConfiguration : IEntityTypeConfiguration<ItemizedProduct>
    {
        public void Configure(EntityTypeBuilder<ItemizedProduct> builder)
        {
            builder.Property(x => x.Description).HasMaxLength(200);
            builder.Property(x => x.Description).HasMaxLength(100);

            builder.ToTable("itemizado_producto");

            builder.Property(x => x.CreateAt)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(GETUTCDATE())");

            builder.HasOne(d => d.Budget)
              .WithMany(p => p.ItemizedProducts) // declarar campo/atributo  en Budget class -->  public virtual ICollection<ItemizedProduct> ItemizedProducts { get; set; }
              .HasForeignKey(d => d.BudgetId)
              .OnDelete(DeleteBehavior.ClientSetNull)
              .HasConstraintName("FK_ItemizedProducts_Budget");

        }
    }
}
