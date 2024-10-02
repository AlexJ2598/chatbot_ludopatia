namespace chatbot_ludo.Web.DTO
{
    public class ConsejoDTO
    {
        //Para un consumo más limpio de la API en formato JSON.
        public int ID_Consejo { get; set; }
        public string Texto_Consejo { get; set; }
        public string Categoria { get; set; }
        public int Grado_Recomendacion { get; set; }
        public DateTime Fecha_Creacion { get; set; }

        //Exponemos la información básica del usuario en lugar de toda la entidad.
        public string UserName { get; set; }
        public string UserEmail { get; set; }
    }
}
