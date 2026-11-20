using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stone.Entities;

namespace Stone.Persistence.Configurations
{
    public class FileDriveConfiguration : IEntityTypeConfiguration<FileDrive>
    {
        public void Configure(EntityTypeBuilder<FileDrive> builder)
        {
            builder.Property(x => x.FileName).HasMaxLength(200);
            builder.Property(x => x.FileExtention).HasMaxLength(10);
            builder.Property(x => x.ContentType).HasMaxLength(100);
            builder.Property(x => x.FileId).HasMaxLength(100);
            builder.Property(x => x.FolderParentId).HasMaxLength(100);
            builder.HasQueryFilter(x => x.Active);

            builder.ToTable("archivos");

            builder.Property(x => x.CreateAt)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(GETUTCDATE())");
        }
    }
}
