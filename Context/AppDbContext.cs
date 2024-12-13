using Microsoft.EntityFrameworkCore;
using WebAPi.Models;

namespace WebAPi.Context
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
            
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<SaleItem> SaleItems { get; set; }
        public DbSet<Sale> Sales { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relación productos categorias
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category) // Un producto tiene una categoría
                .WithMany(c => c.Products) // Una categoría tiene muchos productos
                .HasForeignKey(p => p.CategoryId) // Clave foránea en la tabla Product
                .OnDelete(DeleteBehavior.Cascade); // Configura eliminación en cascada

            // Relacion usuarios roles
            modelBuilder.Entity<Users>()
           .HasOne(u => u.Role) // Un usuario tiene un rol
           .WithMany(r => r.Users) // Un rol tiene muchos usuarios
           .HasForeignKey(u => u.RoleId) // Clave foránea
           .OnDelete(DeleteBehavior.Restrict); // Opcional: evita eliminar roles con usuarios asociados

            // Relación Sale y SaleItem
            modelBuilder.Entity<Sale>()
                .HasMany(s => s.SaleItems) // Una venta tiene muchos artículos
                .WithOne(si => si.Sale) // Un artículo pertenece a una venta
                .HasForeignKey(si => si.SaleId) // Clave foránea en SaleItem
                .OnDelete(DeleteBehavior.Cascade); // Si se elimina una venta, también los artículos

            base.OnModelCreating(modelBuilder);
        }

    }
}
