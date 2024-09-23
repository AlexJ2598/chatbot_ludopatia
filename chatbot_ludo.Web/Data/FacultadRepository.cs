namespace chatbot_ludo.Web.Data
{
    using Entities;
    public class FacultadRepository : GenericRepository<Facultad>, IFacultadRepository //Implementamos el repositorio generico con los consejos e implementamos el IConsejoRepository.
    {
        public FacultadRepository(DataContext context) : base(context)
        {
            //De esta manera ya tenemos los metodos para adicionar, modificar y borrar facultades.
        }
    }
}
