using chatbot_ludo.Web.Data;
using chatbot_ludo.Web.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor
builder.Services.AddControllersWithViews();

// Configurar Entity Framework Core para usar SQL Server
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar SeedDB como servicio
builder.Services.AddTransient<SeedDB>(); //Esto es para registrar la inyeccion de la clase en la base de datos. 

//Añadimos el manejo de usuarios en cuanto a las restricciones que necesitan las contraseñas:
// Configurar Identity con personalizaciones
builder.Services.AddIdentity<User, IdentityRole>(cfg =>
{
    cfg.User.RequireUniqueEmail = true;
    cfg.Password.RequireDigit = false;
    cfg.Password.RequiredUniqueChars = 0;
    cfg.Password.RequireLowercase = false;
    cfg.Password.RequireNonAlphanumeric = false;
    cfg.Password.RequireUppercase = false;
    cfg.Password.RequiredLength = 6; //Minimo de 6 caracteres o longitud
})
.AddEntityFrameworkStores<DataContext>()
.AddDefaultTokenProviders(); // Agrega los proveedores de tokens por defecto para restablecimiento de contraseña, etc.

//Explicación de los Cambios:
//builder.Services.AddIdentity:
//Aquí estamos registrando los servicios de Identity con las mismas configuraciones que antes.
//cfg.User.RequireUniqueEmail = true: Requiere que cada usuario tenga un correo electrónico único.
//cfg.Password.*: Estas configuraciones personalizan los requisitos de la contraseña.
//AddEntityFrameworkStores<DataContext>:
//Asocia Identity con Entity Framework Core y el contexto DataContext para que gestione el almacenamiento de los usuarios.
//AddDefaultTokenProviders:
//Esto agrega los proveedores de tokens por defecto que Identity usa para funcionalidades como el restablecimiento de contraseñas y la confirmación por correo electrónico.
//Autenticación y Autorización:
//app.UseAuthentication();: Este middleware asegura que las solicitudes de usuarios autenticados se procesen correctamente.
//app.UseAuthorization();: Este middleware asegura que solo los usuarios con los permisos adecuados puedan acceder a ciertos recursos.

// Registrar Repository como un servicio Scoped
builder.Services.AddScoped<IRepository, Repository>(); //Añadimos esta parte para inyectar la implementación de la clase Repository.

var app = builder.Build();

// Llamar a SeedAsync
SeedData(app);

// Configuración del pipeline de solicitudes HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication(); // Agrega el middleware para la autenticación
app.UseAuthorization();  // Agrega el middleware para la autorización

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

// Método para ejecutar SeedAsync
void SeedData(IHost app)
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;

        try
        {
            var seedDB = services.GetRequiredService<SeedDB>();
            seedDB.SeedAsync().Wait();
        }
        catch (Exception ex)
        {
            // Aquí puedes manejar cualquier excepción que ocurra durante el seeding
            Console.WriteLine("Ocurrió un error al inicializar la base de datos: " + ex.Message);
        }
    }
}

