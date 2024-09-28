namespace chatbot_ludo.Web.Controllers.API
{
    using chatbot_ludo.Web.Data;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[Controller]")] //Ruteamos, cuando se publique el sitio web la direccion es api/controlador.
    public class ConsejosController : Controller
    {
        private readonly IConsejoRepository consejoRepository; //Inicializamos la interfaz.

        //Inyectamos el repositorio por medio del constructor.
        public ConsejosController(IConsejoRepository consejoRepository) //Inyectamos por medio de la interface.
        {
            this.consejoRepository = consejoRepository;
        }

        //Metodo para obtener los consejos:
        [HttpGet] //Get del controlador
        public IActionResult GetConsejos() 
        {
            //Podemos hacer las solicitudes en POSTMAN para validar.
            //https://localhost:7197/api/Consejos cambiar direccion en localhost para hacer el metodo GET en POSTMAN.Nos devuelve esto los JSON.
            return Ok(this.consejoRepository.GetAllWithUser()); //De esta manera en la API obtenemos los objetos con el usuario. No lo hacemos en el controller porque no se está ocupando.
        }
    }
}
