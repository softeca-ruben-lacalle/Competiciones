using Competiciones.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Competiciones.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Pais> Paises { get; set; }
        public DbSet<Competicion> Competiciones { get; set; }
        public DbSet<Equipo> Equipos { get; set; }
        public DbSet<Jugador> Jugadores { get; set; }
        public DbSet<Partido> Partidos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Partido>()
                .HasOne(p => p.EquipoLocal)
                .WithMany(e => e.PartidosComoLocal)
                .HasForeignKey(p => p.EquipoLocalId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Partido>()
                .HasOne(p => p.EquipoVisitante)
                .WithMany(e => e.PartidosComoVisitante)
                .HasForeignKey(p => p.EquipoVisitanteId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
