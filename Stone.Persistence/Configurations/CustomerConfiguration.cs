using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stone.Entities;

namespace Stone.Persistence.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.Property(x => x.Rut)
                .HasMaxLength(12);

            builder.Property(x => x.Email)
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.Property(x => x.DisplayName)
                .HasMaxLength(200);


            builder.ToTable("cliente");

            builder.Property(x => x.CreateAt)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(GETUTCDATE())");
        }
    }
}
