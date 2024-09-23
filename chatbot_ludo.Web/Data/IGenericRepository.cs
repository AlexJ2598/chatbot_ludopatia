namespace chatbot_ludo.Web.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    public interface IGenericRepository<T> where T : class //Un repositorio de T, donde T es una clase. Osea, podemos cambiar T por cualquier cosa (consejos, usuarios, etc).
    {
        IQueryable<T> GetAll(); //Nos devuelve una lista de T, siendo T lo que necesitemos

        Task<T> GetByIdAsync(int id); //Nos devuelte un T (cualquier cosa).

        Task CreateAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);

        Task<bool> ExistAsync(int id);
    }

}
