namespace chatbot_ludo.Web.Data
{
    using Entities;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class DataContext : IdentityDbContext<User> //Modificamos el contexto, que incluye las tablas de usuarios que maneja la seguridad integrada.
    {
        public DbSet<Consejo> Consejos { get; set; }
        //Añadimos las facultadede
        public DbSet<Facultad> Facultades { get; set; } //Corremos el comando. No olvidar guardar antes de correr el comando.

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de la clave primaria para Consejo
            modelBuilder.Entity<Consejo>()
                .HasKey(c => c.ID_Consejo);  // Define la clave primaria explícitamente

            // Mapeo de ID_Consejo a Id
            modelBuilder.Entity<Consejo>()
                .Property(c => c.ID_Consejo)
                .HasColumnName("Id");

            // Configuración de la relación Consejo-User
            modelBuilder.Entity<Consejo>()
                .HasOne(c => c.User)  // Un consejo tiene un usuario
                .WithMany(u => u.Consejos)  // Un usuario tiene muchos consejos
                .HasForeignKey(c => c.UserId)  // Clave foránea en la tabla Consejo
                .OnDelete(DeleteBehavior.Restrict);  // Evita la eliminación en cascada si es necesario
            // Evitar eliminación en cascada en todas las relaciones
            var cascadeFKs = modelBuilder.Model
                .GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade); //De está manera no borramos los Consejos si elimininamos un usuario que haya cargado.

            foreach (var fk in cascadeFKs)
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            }
            //Configuraciones adicionales. 
        }
    }
}

