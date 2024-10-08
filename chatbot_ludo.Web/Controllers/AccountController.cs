namespace chatbot_ludo.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Helpers;
    using Models;
    using Microsoft.AspNetCore.Identity;
    using Data.Entities;

    public class AccountController : Controller //Heredamos para que indiquemos que es un controlador.
    {
        private readonly IUserHelper userHelper; //Inicializamos.

        //Inyectamos el UserHelper.
        public AccountController(IUserHelper userHelper)
        {
            this.userHelper = userHelper;
        }
        //Metodo login para cuando lo creemos. Este es el GET
        public IActionResult Login() //Todos los controladores devuelven un ActionResult.
        {
            //Validamos si no está logeado. 
            if (this.User.Identity.IsAuthenticated) //Al heredar de la clase controller ya hay un atributo llamado Identity e isAuth.
            {
                return this.RedirectToAction("Index", "Home");//Si está auth si va al Index-
            }

            return this.View(); //No login, manda a la vista para logear.
        }
        //Post
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = await this.userHelper.GetUserByEmailAsync(model.Username);

                if (user == null)
                {
                    this.ModelState.AddModelError(string.Empty, "User does not exist.");
                    return this.View(model);
                }

                // Verificar si la contraseña es válida
                var isPasswordValid = await this.userHelper.CheckPasswordAsync(user, model.Password);
                if (!isPasswordValid)
                {
                    this.ModelState.AddModelError(string.Empty, "Invalid password.");
                    return this.View(model);
                }

                // Intentar iniciar sesión
                var result = await this.userHelper.LoginAsync(model);
                if (result.Succeeded)
                {
                    if (this.Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return this.Redirect(this.Request.Query["ReturnUrl"].First());
                    }

                    return this.RedirectToAction("Index", "Home");
                }

                this.ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            this.ModelState.AddModelError(string.Empty, "Failed to login.");
            return this.View(model);
        }




        //Metodo logout
        public async Task<IActionResult> Logout()
        {
            await this.userHelper.LogoutAsync(); //Deslogeamos
            return this.RedirectToAction("Index", "Home"); //Vamos al index
        }
        //Metodo para registrar.
        public IActionResult Register() //Se llama igual que en el formulario.Debe coincidir.
        {
            return this.View();
        }
        //Metodo POST
        [HttpPost]
        public async Task<IActionResult> Register(RegisterNewUserViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                //Validamos si el usuario ya existe
                var user = await this.userHelper.GetUserByEmailAsync(model.Username);
                if (user == null)
                {
                    //Si pasa creamos el usuario con los campos dados.
                    user = new User
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Username,
                        UserName = model.Username
                    };

                    var result = await this.userHelper.AddUserAsync(user, model.Password);
                    if (result != IdentityResult.Success)
                    {
                        this.ModelState.AddModelError(string.Empty, "The user couldn't be created.");
                        return this.View(model);
                    }

                    //Si lo crea correctamente pasamos a logearlo.
                    var loginViewModel = new LoginViewModel
                    {
                        Password = model.Password,
                        RememberMe = false,
                        Username = model.Username
                    };

                    var result2 = await this.userHelper.LoginAsync(loginViewModel);

                    if (result2.Succeeded)
                    {
                        return this.RedirectToAction("Index", "Home");
                    }

                    this.ModelState.AddModelError(string.Empty, "The user couldn't be login.");
                    return this.View(model);
                }

                this.ModelState.AddModelError(string.Empty, "The username is already registered.");
            }

            return this.View(model);
        }


    }
}
