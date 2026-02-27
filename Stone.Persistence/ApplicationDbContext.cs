using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Stone.Entities.Info;
using System.Reflection;

namespace Stone.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        //Fluent API
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.Entity<ConcertInfo>().HasNoKey();
            modelBuilder.Entity<ReportInfo>().HasNoKey().Property(e => e.Total).HasPrecision(18, 4);


            modelBuilder.Entity<User>(x => x.ToTable("User"));
            modelBuilder.Entity<IdentityRole>(x => x.ToTable("Role"));
            modelBuilder.Entity<IdentityUserRole<string>>(x => x.ToTable("UserRole"));
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseLazyLoadingProxies();
            }

        }

        //public DbSet<Genre> Genres { get; set; }
    }
}
