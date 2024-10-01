namespace chatbot_ludo.Common.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    public class Consejo
    {
        [JsonProperty("$id")]
        public long Id { get; set; }

        [JsonProperty("iD_Consejo")]
        public long IDConsejo { get; set; }

        [JsonProperty("texto_Consejo")]
        public string TextoConsejo { get; set; }

        [JsonProperty("categoria")]
        public string Categoria { get; set; }

        [JsonProperty("grado_Recomendacion")]
        public long GradoRecomendacion { get; set; }

        [JsonProperty("fecha_Creacion")]
        public DateTimeOffset FechaCreacion { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("userId")]
        public Guid UserId { get; set; }
    }
}
