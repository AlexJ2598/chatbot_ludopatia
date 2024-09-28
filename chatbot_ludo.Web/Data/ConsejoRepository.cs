namespace chatbot_ludo.Web.Data
{
    using System.Linq;
    using Entities;
    using Microsoft.EntityFrameworkCore;

    public class ConsejoRepository : GenericRepository<Consejo>, IConsejoRepository //Implementamos el repositorio generico con los consejos e implementamos el IConsejoRepository.
    {
        private readonly DataContext context;

        public ConsejoRepository(DataContext context) : base(context) 
        {
            this.context = context;
        }

        //Lo podemos ordenar del otro lado. Retorna con usuarios.
        public IQueryable GetAllWithUser()
        {
            return this.context.Consejos.Include(c => c.User);
        }
    }
}
