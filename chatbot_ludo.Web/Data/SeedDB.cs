namespace chatbot_ludo.Web.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Entities;
    using Microsoft.EntityFrameworkCore;

    public class SeedDB
    {
        private readonly DataContext context;

        public SeedDB(DataContext context)
        {
            this.context = context;
        }

        public async Task SeedAsync()
        {
            await this.context.Database.MigrateAsync();

            if (!this.context.Consejos.Any())
            {
                this.AddConsejo("Establece límites claros de tiempo y dinero antes de participar en actividades de juego, y asegúrate de respetarlos siempre.",
                    "Prevencion",3,DateTime.Now);
                this.AddConsejo("Si sientes que el juego está afectando negativamente tu vida, no dudes en buscar apoyo en amigos, familiares o profesionales de la salud.",
                    "Apoyo",3,DateTime.Now);
                this.AddConsejo("Identifica y participa en actividades alternativas y hobbies que te apasionen para ocupar tu tiempo de manera positiva.",
                    "Prevencion",2,DateTime.Now);
                await this.context.SaveChangesAsync(); //Aqui salvamos.
            }
        }

        private void AddConsejo(string consejo, string categoria, int grado_recomendacion, DateTime fecha_creacion)
        {
            this.context.Consejos.Add(new Consejo
            {
                Texto_Consejo = consejo,
                Categoria = categoria,
                Grado_Recomendacion = grado_recomendacion,
                Fecha_Creacion = fecha_creacion,
            });
        }

    }
}
