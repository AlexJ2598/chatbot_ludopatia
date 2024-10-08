namespace chatbot_ludo.Web.Helpers
{
    using System.Threading.Tasks;
    using chatbot_ludo.Web.Models;
    using Data.Entities;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Logging;

    public class UserHelper : IUserHelper
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager; //Lo inicializamos también para implementarlo. Mismo, no se inyectan en el program porque son nativas del core.
        private readonly ILogger<UserHelper> _logger; //Para el correcto manejo de inicio de sesiones.

        public UserHelper(UserManager<User> userManager, SignInManager<User> signInManager, ILogger<UserHelper> logger) //Inyectamos en sign para logear y deslogear y el ILoggger para capturar Logs.
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this._logger = logger;
        }

        public async Task<IdentityResult> AddUserAsync(User user, string password)
        {
            return await this.userManager.CreateAsync(user, password); //Agregamos usuario. Creamos con user y password. Esta es una interface, toca mapearla en program,
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await this.userManager.FindByEmailAsync(email); //Devuelme el email de manera async.
        }

        //Las nuevas firmas que implementamos en la interface.
        public async Task<SignInResult> LoginAsync(LoginViewModel model)
        {
            // Intentamos obtener el usuario basado en el correo electrónico.
            var user = await this.userManager.FindByEmailAsync(model.Username); //Modificamos esta parte para buscar por el correo que es lo que estamos logeando.

            if (user == null)
            {
                // Si no encontramos el usuario, registramos un warning.
                _logger.LogWarning("No se encontró el usuario con el correo: {Email}", model.Username);
                return SignInResult.Failed;
            }

            // Registramos que estamos intentando iniciar sesión para el usuario.
            _logger.LogInformation("Intentando iniciar sesión para el usuario: {Email}", model.Username);

            // Realizamos el inicio de sesión basado en el UserName del usuario encontrado.
            var result = await this.signInManager.PasswordSignInAsync(
                user.UserName,  // Usamos el UserName del usuario encontrado
                model.Password,
                model.RememberMe,
                false); // El cuarto parámetro es para habilitar bloqueo en caso de intentos fallidos.

            // Si el inicio de sesión no es exitoso, registramos un warning.
            if (!result.Succeeded)
            {
                _logger.LogWarning("Error al intentar iniciar sesión para el usuario: {Email}", model.Username); //Todos estos logs van a la consola.
            }
            else
            {
                // Registramos un mensaje de éxito si el inicio de sesión fue exitoso.
                _logger.LogInformation("Inicio de sesión exitoso para el usuario: {Email}", model.Username);
            }

            // Devolvemos el resultado del intento de inicio de sesión.
            return result;
        }

        public async Task LogoutAsync()
        {
            await this.signInManager.SignOutAsync();
        }

        //Verificamos password.
        public async Task<bool> CheckPasswordAsync(User user, string password)
        {
            return await this.userManager.CheckPasswordAsync(user, password);
        }
    }

}
