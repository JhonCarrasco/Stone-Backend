using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stone.Entities.Budget;

namespace Stone.Persistence.Configurations
{
    public class ItemizedServiceConfiguration : IEntityTypeConfiguration<ItemizedService>
    {
        public void Configure(EntityTypeBuilder<ItemizedService> builder)
        {
            builder.Property(x => x.Description).HasMaxLength(200);

            builder.ToTable("itemizado_servicio");

            builder.Property(x => x.CreateAt)
            .HasColumnType("datetime")
            .HasDefaultValueSql("(GETUTCDATE())");


            builder.HasOne(d => d.Budget)
            .WithMany(p => p.ItemizedServices) // declarar campo/atributo  en Budget class -->  public virtual ICollection<ItemizedService> ItemizedServices { get; set; }
            .HasForeignKey(d => d.BudgetId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_ItemizedServices_Budget");
        }
    }
}
