using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stone.Entities;

namespace Stone.Persistence.Configurations
{
    public class MaterialConfiguration : IEntityTypeConfiguration<Material>
    {
        public void Configure(EntityTypeBuilder<Material> builder)
        {
            builder.Property(x => x.ProductCode).HasMaxLength(200);
            builder.Property(x => x.Description).HasMaxLength(200);
            //builder.HasQueryFilter(x => x.Active);

            builder.ToTable("material");

            builder.Property(x => x.CreateAt)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(GETUTCDATE())");
        }
    }
}
