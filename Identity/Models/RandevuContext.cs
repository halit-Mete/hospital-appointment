using Microsoft.EntityFrameworkCore;
using Identity.Models;

namespace Identity.Models
{
    public class RandevuContext : DbContext
    {
        public RandevuContext(DbContextOptions<RandevuContext> options) : base(options)
        {
        }

        public DbSet<Poliklinik>? Poliklinikler { get; set; }
        public DbSet<Doktor>? Doktorlar { get; set; }
        public DbSet<Randevu>? Randevular { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Doktor>()
                .HasOne(d => d.Poliklinik)
                .WithMany(p => p.Doktorlar)
                .HasForeignKey(d => d.PoliklinikId);

            modelBuilder.Entity<Randevu>()
                .HasOne(r => r.Doktor)
                .WithMany()
                .HasForeignKey(r => r.DoktorId);
        }
    }
}
