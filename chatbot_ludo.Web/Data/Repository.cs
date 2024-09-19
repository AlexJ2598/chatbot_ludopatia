namespace chatbot_ludo.Web.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Entities;
    //using Common.Models;

    public class Repository : IRepository
    {
        //Vamos a ingresar por medio de una interfaz a este repositorio.
        //Click derecho a la clase, acciones rapidas, extraer interface
        //Aquí vamos a inyectar la base de datos.
        private readonly DataContext context;
        public Repository(DataContext context) //Así inyectamos
        {
            this.context = context;

        }

        //Vamos aquí a tener todos los metodos.
        //Clase generica para obtener el texto de los consejos
        public IEnumerable<Consejo> GetConsejos()
        {
            return this.context.Consejos.OrderBy(c => c.Texto_Consejo); //Por linq le estamos pidiendo que nos retorne los textos de consejos.
            //p es cualquier cosa que le pido. P es una expresion lamda. Usamos c por ser consejos.
        }

        public Consejo GetConsejo(int id)
        {
            return this.context.Consejos.Find(id); //Para encontrar un consejo por medio del id. Metodo Find.
        }

        public void AddConsejo(Consejo consejo)
        {
            this.context.Consejos.Add(consejo); //Añadimos un consejo
        }

        public void UpdateConsejo(Consejo consejo) //Actualizamos un consejo
        {
            this.context.Update(consejo); //Recordar respetar siempre la sintaxis.
        }

        public void RemoveConsejo(Consejo consejo) //Eliminamos
        {
            this.context.Consejos.Remove(consejo);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await this.context.SaveChangesAsync() > 0; //Metodo async para salvar los datos. Devuelve un bool, 1 si es para salvar.
        }

        public bool ConsejoExists(int id)
        {
            return this.context.Consejos.Any(c => c.ID_Consejo == id); //Revisamos si no hay un dato repetido. Usamos any si cualquier consejo de la base de datos se repite.
        }

    }
}
