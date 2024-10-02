
namespace chatbot_ludo.Web.Data
{
    using Entities;
    using System.Linq;
    public interface IConsejoRepository : IGenericRepository<Consejo>
    {
        // Método que devuelve todos los consejos junto con los usuarios
        IQueryable<Consejo> GetAllWithUser(); // Cambiamos a IQueryable<Consejo>
    }
}

