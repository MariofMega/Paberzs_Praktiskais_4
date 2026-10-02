using Microsoft.EntityFrameworkCore;

namespace Paberzs_Praktiskais_3.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Developer> Developers { get; set; }
        public DbSet<Platform> Platforms { get; set; }
        public DbSet<Game> Games { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Developer>()
                .HasMany(d => d.Games)
                .WithOne(g => g.Developer)
                .HasForeignKey(g => g.DeveloperId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Platform>()
                .HasMany(p => p.Games)
                .WithOne(g => g.Platform)
                .HasForeignKey(g => g.PlatformId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}