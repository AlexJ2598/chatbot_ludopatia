namespace chatbot_ludo.Web.Data
{
    using Entities;
    public class ConsejoRepository : GenericRepository<Consejo>, IConsejoRepository //Implementamos el repositorio generico con los consejos e implementamos el IConsejoRepository.
    {
        public ConsejoRepository(DataContext context) : base(context) 
        { 
            
        }
    }
}
