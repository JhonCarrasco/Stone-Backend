using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stone.Entities;

namespace Stone.Persistence.Configurations
{
    public class ContactConfiguration : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.Property(x => x.BusinessActivity)
                            .HasMaxLength(100)
                            .IsUnicode(false);
            builder.Property(x => x.Phone)
                            .HasMaxLength(12)
                            .IsUnicode(false);
            builder.Property(x => x.Email)
                            .HasMaxLength(100)
                            .IsUnicode(false);

            builder.ToTable("contacto");

            builder.HasOne(d => d.Provider)
              .WithMany(p => p.Contacts) // declarar campo/atributo  en Provider class -->  public virtual ICollection<Contact> Contacts { get; set; }   y  en Contact class --> public int? ProviderId { get; set; }  y  public virtual Provider Provider { get; set; }
              .HasForeignKey(d => d.ProviderId)
              .OnDelete(DeleteBehavior.ClientSetNull)
              .HasConstraintName("FK_Contacts_Provider");
        }
    }
}
