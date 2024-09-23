namespace chatbot_ludo.Web.Data.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    public class Consejo : IEntity
    {
        public int ID_Consejo {  get; set; } //Lave primaria
        // Implementación de la interfaz IEntity con un mapeo hacia ID_Consejo. De esta manera no necesitamos cambiar ID_Consejo por Id
        int IEntity.Id
        {
            get => ID_Consejo;
            set => ID_Consejo = value;
        }
        [MaxLength(250, ErrorMessage = "The field {0} only can contain {1} characters length.")] //Anotaciones para limitar el consejo a 150 caracteres max
        [Required] //Requerido. No puede haber consejos vacios.
        public string Texto_Consejo { get; set;}
        [MaxLength(50)]
        public string Categoria {  get; set; } //prevesion, sintomas, apoyo, etc.
        public int Grado_Recomendacion { get; set; } //1 bajo, 2 medium, 3 hight
        public DateTime Fecha_Creacion {  get; set; }

        //Relacionamosla tabla de consejos con la de usuarios.
        public User User { get; set; } //Configuramos de uno a varios.
        
        // Clave foránea para la relación con el usuario
        //[Required]  // Indica que la clave foránea es obligatoria. De momento no es requerida debido a que no tenemos usuario logeado.
        public string UserId { get; set; }  // Clave foránea


    }
}
