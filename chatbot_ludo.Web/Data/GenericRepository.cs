namespace chatbot_ludo.Web.Data
{
    using System.Linq;
    using System.Threading.Tasks;
    using Entities;
    using Microsoft.CodeAnalysis;
    using Microsoft.EntityFrameworkCore;

    public class GenericRepository<T> : IGenericRepository<T> where T : class, IEntity //Implementamos la interface Generica de T.
    {
        private readonly DataContext context;

        public GenericRepository(DataContext context)
        {
            this.context = context;
        }

        public IQueryable<T> GetAll()
        {
            return this.context.Set<T>().AsNoTracking(); //Establece la lista que vamos a devolver, sean consejos, usuarios, etc.
        }

        public async Task<T> GetByIdAsync(int id)
        {
            //Si todas las entidades implementan la interfaz IEntity
            //y tienen una propiedad de clave primaria diferente(como ID_Consejo en lugar de Id), se hace un enfoque más general.
            //Explicacion: 
            //FindEntityType(typeof(T)): Obtiene la metadata de la entidad T para encontrar cuál es la clave primaria.
            //FindPrimaryKey().Properties[0].Name: Recupera el nombre de la propiedad clave primaria(en el caso de Consejo, sería ID_Consejo).
            //EF.Property<int>(e, keyName): Permite hacer la consulta utilizando la clave primaria correcta para la entidad.
            var keyName = this.context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties[0].Name;

            return await this.context.Set<T>()
                .AsNoTracking()
                .FirstOrDefaultAsync(e => EF.Property<int>(e, keyName) == id);
        }


        public async Task CreateAsync(T entity)
        {
            await this.context.Set<T>().AddAsync(entity);
            await SaveAllAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            this.context.Set<T>().Update(entity);
            await SaveAllAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            this.context.Set<T>().Remove(entity);
            await SaveAllAsync();
        }

        public async Task<bool> ExistAsync(int id)
        {
            var keyName = this.context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties[0].Name;

            return await this.context.Set<T>()
                .AnyAsync(e => EF.Property<int>(e, keyName) == id);
        }


        public async Task<bool> SaveAllAsync()
        {
            return await this.context.SaveChangesAsync() > 0;
        }
    }
    //Todo esto se hace a modo de ahorrar todos los metodos para cada interface o modelo (Entitie) que vayamos a agregar al proyecto.
    //Vamos a hacerlo con un ejemplo de Facultades.

}
