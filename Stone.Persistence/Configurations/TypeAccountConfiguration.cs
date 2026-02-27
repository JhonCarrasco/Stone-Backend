using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stone.Entities.Generic;

namespace Stone.Persistence.Configurations
{
    public class TypeAccountConfiguration : IEntityTypeConfiguration<TypeAccount>
    {
        public void Configure(EntityTypeBuilder<TypeAccount> builder)
        {
            builder.Property(x => x.Description)
                            .HasMaxLength(100)
                            .IsUnicode(false);

            builder.ToTable("tipo_cuenta");

            builder.Property(x => x.CreateAt)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(GETUTCDATE())");
        }
    }
}
