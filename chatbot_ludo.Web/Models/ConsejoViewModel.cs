namespace chatbot_ludo.Web.Models
{

    using System;
    using System.ComponentModel.DataAnnotations;

    public class ConsejoViewModel
    {
        public int ID_Consejo { get; set; }

        [Required]
        [MaxLength(250, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        public string Texto_Consejo { get; set; }

        [Required]
        [MaxLength(50)]
        public string Categoria { get; set; }

        [Required]
        public int Grado_Recomendacion { get; set; }

        [Required]
        public DateTime Fecha_Creacion { get; set; }

    }
}

