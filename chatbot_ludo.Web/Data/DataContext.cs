namespace chatbot_ludo.Web.Data
{
    using chatbot_ludo.Web.Data.Entities;
    using Microsoft.EntityFrameworkCore;

    public class DataContext : DbContext
    {
        public DbSet<Consejo> Consejos { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Consejo>()
                .HasKey(c => c.ID_Consejo);  // Define la clave primaria explícitamente

            // Configuración adicional si es necesaria
        }
    }
}

