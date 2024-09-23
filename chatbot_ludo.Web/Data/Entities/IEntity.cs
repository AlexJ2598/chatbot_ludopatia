namespace chatbot_ludo.Web.Data.Entities
{
    public interface IEntity
    {
        int Id { get; set; } //Podemos añadir atributos a las interface. Cualquier clase que la implemente esta obligada a declarar un Id.
    }
}
