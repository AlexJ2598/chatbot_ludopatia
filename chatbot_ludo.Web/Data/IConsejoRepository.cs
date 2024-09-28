namespace chatbot_ludo.Web.Data
{
    using Entities;
    public interface IConsejoRepository :IGenericRepository<Consejo> //La interface de consejo va a hacer una implementación de la interface generica. Va a devolver consejos.
    {
        //Vamos a añadir los metodos personalizados. Metodos que le van a servir a Consejo, no al resto de cosas.
        IQueryable GetAllWithUser(); //Este metodo lo crea en el repositorio.
    }
}
