namespace chatbot_ludo.Web.Data.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    public class Consejo
    {
        public int ID_Consejo {  get; set; } //Lave primaria
        public string? Texto_Consejo { get; set;}
        public string Categoria {  get; set; } //prevesion, sintomas, apoyo, etc.
        public int Grado_Recomendacion { get; set; } //1 bajo, 2 medium, 3 hight
        public DateTime Fecha_Creacios {  get; set; }

    }
}
