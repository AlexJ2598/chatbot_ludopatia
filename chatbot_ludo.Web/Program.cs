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

//A�adimos el manejo de usuarios en cuanto a las restricciones que necesitan las contrase�as:
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
.AddDefaultTokenProviders(); // Agrega los proveedores de tokens por defecto para restablecimiento de contrase�a, etc.

//Explicaci�n de los Cambios:
//builder.Services.AddIdentity:
//Aqu� estamos registrando los servicios de Identity con las mismas configuraciones que antes.
//cfg.User.RequireUniqueEmail = true: Requiere que cada usuario tenga un correo electr�nico �nico.
//cfg.Password.*: Estas configuraciones personalizan los requisitos de la contrase�a.
//AddEntityFrameworkStores<DataContext>:
//Asocia Identity con Entity Framework Core y el contexto DataContext para que gestione el almacenamiento de los usuarios.
//AddDefaultTokenProviders:
//Esto agrega los proveedores de tokens por defecto que Identity usa para funcionalidades como el restablecimiento de contrase�as y la confirmaci�n por correo electr�nico.
//Autenticaci�n y Autorizaci�n:
//app.UseAuthentication();: Este middleware asegura que las solicitudes de usuarios autenticados se procesen correctamente.
//app.UseAuthorization();: Este middleware asegura que solo los usuarios con los permisos adecuados puedan acceder a ciertos recursos.

// Registrar Repository como un servicio Scoped
builder.Services.AddScoped<IRepository, Repository>(); //A�adimos esta parte para inyectar la implementaci�n de la clase Repository.

var app = builder.Build();

// Llamar a SeedAsync
SeedData(app);

// Configuraci�n del pipeline de solicitudes HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication(); // Agrega el middleware para la autenticaci�n
app.UseAuthorization();  // Agrega el middleware para la autorizaci�n

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

// M�todo para ejecutar SeedAsync
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
            // Aqu� puedes manejar cualquier excepci�n que ocurra durante el seeding
            Console.WriteLine("Ocurri� un error al inicializar la base de datos: " + ex.Message);
        }
    }
}

