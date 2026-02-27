using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stone.Entities.Generic;

namespace Stone.Persistence.Configurations
{
    public class ContactConfiguration : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            //builder.Property(x => x.Email)
            //                .HasMaxLength(50)
            //                .IsUnicode(false);
            //builder.Property(x => x.Role)
            //                .HasMaxLength(100)
            //                .IsUnicode(false);
            //builder.Property(x => x.Phone)
            //                .HasMaxLength(12)
            //                .IsUnicode(false);

            builder.ToTable("contacto");
        }
    }
}
