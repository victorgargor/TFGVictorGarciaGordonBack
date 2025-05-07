using EjerciciosVictorAPI.Controllers;
using EjerciciosVictorAPI.Entidades;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EjerciciosVictorAPI.Datos
{
    public class ApplicationDbContext : IdentityDbContext // Clase a partir de la cual configuro las tablas de mi BD
    {
        // Para realizar configuraciones de EntityFrameworkCore fuera de esta clase
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        // Para indicar que quiero que se cree una tabla en mi BD a partir de las propiedades de la clase Cliente
        public DbSet<Cliente> Clientes { get; set; }

        // Para indicar que quiero que se cree una tabla en mi BD a partir de las propiedades de la clase Recibo
        public DbSet<Recibo> Recibos { get; set; }

        public DbSet<Permiso> Permisos { get; set; }
        public DbSet<RolPermiso> RolPermisos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<RolPermiso>()
                .HasKey(rp => new { rp.RolId, rp.PermisoId });

            builder.Entity<RolPermiso>()
                .HasOne(rp => rp.Rol)
                .WithMany()
                .HasForeignKey(rp => rp.RolId);

            builder.Entity<RolPermiso>()
                .HasOne(rp => rp.Permiso)
                .WithMany(p => p.RolPermisos)
                .HasForeignKey(rp => rp.PermisoId);
        }
    }
}