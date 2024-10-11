namespace chatbot_ludo.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Helpers;
    using Models;
    using Microsoft.AspNetCore.Identity;
    using Data.Entities;
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authentication;

    public class AccountController : Controller //Heredamos para que indiquemos que es un controlador.
    {
        private readonly IUserHelper userHelper; //Inicializamos.
        private readonly IConfiguration configuration; //Inicializamos para la configuración de las token.

        //Inyectamos el UserHelper.
        public AccountController(IUserHelper userHelper, IConfiguration configuration)
        {
            this.userHelper = userHelper;
            this.configuration = configuration;
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
                    // Crear el ClaimsPrincipal manualmente para establecer la cookie
                    var userPrincipal = await this.userHelper.GetUserPrincipalAsync(user);

                    // Configurar las propiedades de autenticación para la cookie
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = model.RememberMe, // Si el usuario seleccionó "Remember Me", la sesión será persistente
                        ExpiresUtc = model.RememberMe ? DateTime.UtcNow.AddDays(15) : DateTime.UtcNow.AddHours(1) // Si "Remember Me" está activado, la cookie durará 15 días, de lo contrario, 1 hora
                    };

                    // Crear la cookie de autenticación
                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        userPrincipal, // Pasamos el ClaimsPrincipal del usuario
                        authProperties); // Aplicamos las propiedades de autenticación

                    // Redirigir si hay una URL de retorno en la solicitud
                    if (this.Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return this.Redirect(this.Request.Query["ReturnUrl"].First());
                    }

                    // Redirigir al home si no hay URL de retorno
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
            // Cierra la sesión y elimina las cookies de autenticación.
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirige al usuario al inicio o a cualquier página que desees.
            return this.RedirectToAction("Index", "Home");
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

        //Nuevo metodo para cambiar datos o contraseña del usuario.
        //GET
        public async Task<IActionResult> ChangeUser()
        {
            var user = await this.userHelper.GetUserByEmailAsync(this.User.Identity.Name);//Estamos logeados
            var model = new ChangeUserViewModel();
            if (user != null)
            {
                model.FirstName = user.FirstName;
                model.LastName = user.LastName;
            }

            return this.View(model);
        }
        //POST
        [HttpPost]
        public async Task<IActionResult> ChangeUser(ChangeUserViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = await this.userHelper.GetUserByEmailAsync(this.User.Identity.Name);//Buscamos el usuario
                if (user != null)
                {
                    user.FirstName = model.FirstName; //Actualizamos.
                    user.LastName = model.LastName;
                    var respose = await this.userHelper.UpdateUserAsync(user); //Actualizamos.
                    if (respose.Succeeded)
                    {
                        this.ViewBag.UserMessage = "User updated!";
                    }
                    else
                    {
                        this.ModelState.AddModelError(string.Empty, respose.Errors.FirstOrDefault().Description);
                    }
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, "User no found.");//Por cualquier cosa no encontro el usuario.
                }
            }

            return this.View(model);
        }
        //GET para CHANGE PASSWORD.
        public IActionResult ChangePassword()
        {
            return this.View();
        }
        //POST.
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = await this.userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                if (user != null)
                {
                    var result = await this.userHelper.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return this.RedirectToAction("ChangeUser");
                    }
                    else
                    {
                        this.ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description);
                    }
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, "User no found.");
                }
            }

            return this.View(model);
        }

        //Metodo para la generación de token: 
        [HttpPost]
        public async Task<IActionResult> CreateToken([FromBody] LoginViewModel model)
        {
            // Verificamos si el modelo es válido (ej: si los campos requeridos están presentes)
            if (this.ModelState.IsValid)
            {
                // Intentamos obtener al usuario con el email proporcionado en el modelo de login
                var user = await this.userHelper.GetUserByEmailAsync(model.Username);

                // Si el usuario existe
                if (user != null)
                {
                    // Validamos si la contraseña es correcta utilizando el método ValidatePasswordAsync
                    var result = await this.userHelper.ValidatePasswordAsync(user, model.Password);

                    // Si la validación de la contraseña fue exitosa
                    if (result.Succeeded)
                    {
                        // Creamos un array de claims que representan la identidad del usuario
                        // Aquí puedes agregar más claims si lo necesitas, como roles o permisos
                        var claims = new[]
                        {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email), // Sub es el claim que representa al sujeto (usuario)
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Jti es el identificador único del token
                };

                        // Obtenemos la clave de firma desde el archivo de configuración
                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.configuration["Tokens:Key"]));

                        // Creamos las credenciales de firma usando la clave y el algoritmo HMAC-SHA256
                        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        // Generamos el token JWT con la información del issuer, audience, claims, tiempo de expiración y las credenciales de firma
                        var token = new JwtSecurityToken(
                            this.configuration["Tokens:Issuer"],   // Emisor del token
                            this.configuration["Tokens:Audience"], // Público objetivo del token
                            claims,                                // Claims del usuario
                            expires: DateTime.UtcNow.AddDays(15),  // Fecha de expiración (15 días en este caso)
                            signingCredentials: credentials        // Credenciales de firma (clave secreta y algoritmo)
                        );

                        // Creamos el resultado con el token JWT en formato string y la fecha de expiración
                        var results = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token), // Convertimos el token a string
                            expiration = token.ValidTo  // Fecha de expiración del token
                        };

                        // Devolvemos una respuesta HTTP 201 (Created) con el token generado y su fecha de expiración
                        return this.Created(string.Empty, results);
                    }
                }
            }

            // Si algo falla (usuario no encontrado, contraseña incorrecta, etc.), devolvemos un BadRequest
            return this.BadRequest();
        }
    }
}
