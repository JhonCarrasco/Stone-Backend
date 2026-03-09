using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stone.Entities;

namespace Stone.Persistence.Configurations
{
    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.Property(x => x.Rut)
                            .HasMaxLength(12)
                            .IsUnicode(false);
            builder.Property(x => x.DisplayName)
                            .HasMaxLength(200)
                            .IsUnicode(false);
       
            builder.Property(x => x.CreateAt)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(GETUTCDATE())");

            builder.ToTable("persona");
        }
    }
}
