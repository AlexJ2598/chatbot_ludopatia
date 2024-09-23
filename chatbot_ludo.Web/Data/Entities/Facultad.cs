namespace chatbot_ludo.Web.Data.Entities
{
    public class Facultad : IEntity
    {
        //Implementamos la interfaz.
        public int Id { get; set; }

        public string Name { get; set; }

        //Ya no tenemos que crear todos los metodos, vamos a ocupar los metodos genericos.
        //Ahora creamos la tabla, vamos al DataContext cada que añadamos algo.
    }
}
