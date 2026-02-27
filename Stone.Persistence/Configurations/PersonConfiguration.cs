using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stone.Entities.Generic;

namespace Stone.Persistence.Configurations
{
    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.Property(x => x.Rut)
                            .HasMaxLength(12)
                            .IsUnicode(false);
            builder.Property(x => x.FirstName)
                            .HasMaxLength(100)
                            .IsUnicode(false);
            builder.Property(x => x.MiddleName)
                            .HasMaxLength(50)
                            .IsUnicode(false);
            builder.Property(x => x.LastName)
                            .HasMaxLength(50)
                            .IsUnicode(false);
            builder.Property(x => x.LastNameMother)
                            .HasMaxLength(50)
                            .IsUnicode(false);
            builder.Property(x => x.CreateAt)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(GETUTCDATE())");

            builder.ToTable("persona");
        }
    }
}
