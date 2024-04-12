using Domain.Models.Authentication;
using Domain.Models.Pratica;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {         
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Pratica> Pratiche { get; set; }
        public DbSet<StoricoPratica> StoricoPratiche { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Pratica>()
                .HasMany(e => e.StoricoPratiche)
                .WithOne(e => e.Pratica)
                .HasForeignKey(e => e.IdPratica)
                .IsRequired();

        }
    }
}
