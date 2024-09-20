namespace chatbot_ludo.Web.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Entities;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public class SeedDB
    {
        private readonly UserManager<User> userManager; //Para el manejo de usuarios.
        private readonly DataContext context;

        //Vamos a hacer una nueva inyeccion de datos.
        public SeedDB(DataContext context, UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager; //Lo pasamos al constructor.
        }

        public async Task SeedAsync()
        {
            await this.context.Database.MigrateAsync();

            //Creamos un usuario.
            var user = await this.userManager.FindByEmailAsync("alexis.hernandez074@gmail.com");
            if (user == null)
            {
                user = new User
                {
                    FirstName = "Alexis",
                    LastName = "Hernandez",
                    Email = "alexis.hernandez074@gmail.com",
                    UserName = "X773660",
                    PhoneNumber = "1234567890",
                };

                var result = await this.userManager.CreateAsync(user, "123456");  //Creamos el usuario con la contraseña 123456
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder"); //Error.
                }
            }

            if (!this.context.Consejos.Any())
            {
                this.AddConsejo("Establece límites claros de tiempo y dinero antes de participar en actividades de juego, y asegúrate de respetarlos siempre.",
                    "Prevencion",3,DateTime.Now, user);
                this.AddConsejo("Si sientes que el juego está afectando negativamente tu vida, no dudes en buscar apoyo en amigos, familiares o profesionales de la salud.",
                    "Apoyo",3,DateTime.Now, user);
                this.AddConsejo("Identifica y participa en actividades alternativas y hobbies que te apasionen para ocupar tu tiempo de manera positiva.",
                    "Prevencion",2,DateTime.Now, user); //Quedo yo como el creador de esos consejos.
                await this.context.SaveChangesAsync(); //Aqui salvamos.
            }
        }

        private void AddConsejo(string consejo, string categoria, int grado_recomendacion, DateTime fecha_creacion, User user)
        {
            this.context.Consejos.Add(new Consejo
            {
                Texto_Consejo = consejo,
                Categoria = categoria,
                Grado_Recomendacion = grado_recomendacion,
                Fecha_Creacion = fecha_creacion,
                User = user
            });
        }

    }
}
