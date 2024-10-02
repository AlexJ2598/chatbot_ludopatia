namespace chatbot_ludo.Web.Controllers.API
{
    using chatbot_ludo.Web.Data;
    using chatbot_ludo.Web.DTO; //Para simplificar el consumo de JSON
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;

    [Route("api/[Controller]")]
    public class ConsejosController : Controller
    {
        private readonly IConsejoRepository consejoRepository;

        public ConsejosController(IConsejoRepository consejoRepository)
        {
            this.consejoRepository = consejoRepository;
        }

        [HttpGet]
        public IActionResult GetConsejos()
        {
            // Obtenemos todos los consejos con el usuario asociado
            var consejos = this.consejoRepository.GetAllWithUser();

            // Mapeamos cada Consejo a ConsejoDTO
            var consejosDTO = consejos.Select(c => new ConsejoDTO
            {
                ID_Consejo = c.ID_Consejo,
                Texto_Consejo = c.Texto_Consejo,
                Categoria = c.Categoria,
                Grado_Recomendacion = c.Grado_Recomendacion,
                Fecha_Creacion = c.Fecha_Creacion,
                UserName = c.User.UserName,  // Tomamos solo el nombre del usuario
                UserEmail = c.User.Email      // Mapeo del correo electrónico del usuario
            }).ToList();

            // Devolvemos la lista de ConsejoDTOs como resultado de la API
            return Ok(consejosDTO);
        }
    }
}



