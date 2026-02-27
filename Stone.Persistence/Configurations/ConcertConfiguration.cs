using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stone.Entities;

namespace Stone.Persistence.Configurations
{
    public class ConcertConfiguration : IEntityTypeConfiguration<Concert>
    {
        public void Configure(EntityTypeBuilder<Concert> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(x => x.Title).HasMaxLength(100);
            builder.Property(x => x.Description).HasMaxLength(200);
            builder.Property(x => x.Place).HasMaxLength(100);
            builder.Property(x => x.DateEvent)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(GETUTCDATE())");
            builder.Property(x => x.ImageUrl)
                .HasMaxLength(300)
                .IsUnicode(false);
            builder.HasIndex(x => x.Title);
            builder.ToTable("Concert", "Musicales");

            //builder.HasOne(d => d.Genre)
            //    .WithMany(p => p.Concerts) // declarar campo/atributo  en Concert class -->  public virtual ICollection<Concert> Concerts { get; set; }
            //    .HasForeignKey(d => d.GenreId)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK_Concerts_Genre");
        }
    }
}
