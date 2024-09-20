namespace chatbot_ludo.Web.Data
{
    using chatbot_ludo.Web.Data.Entities;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class DataContext : IdentityDbContext<User> //Modificamos el contexto, que incluye las tablas de usuarios que maneja la seguridad integrada.
    {
        public DbSet<Consejo> Consejos { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Invoca al método base para configurar las tablas de Identity, esto para User.
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Consejo>()
                .HasKey(c => c.ID_Consejo);  // Define la clave primaria explícitamente

            // Configuración adicional si es necesaria
        }
    }
}

