namespace chatbot_ludo.Common.Models
{
    using System;
    using Newtonsoft.Json;

    public class Consejo
    {
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

        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("userEmail")]
        public string UserEmail { get; set; }

        //Nos pinta el toString de la clase Consejo.
        public override string ToString()
        {
            return $"{this.TextoConsejo}, {this.Categoria}, {this.GradoRecomendacion}"; //Pintamos ahora
        }
    }
}

