using chatbot_ludo.Web.Data;
using chatbot_ludo.Web.Data.Entities;
using chatbot_ludo.Web.Helpers;
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

// Registrar Repository como un servicio Scoped
//builder.Services.AddScoped<IRepository, Repository>(); Se elimina porque ya no vamos a ocupar esa interface hecha especificamente para productos, ahora vamos a implementar las nuevas.
builder.Services.AddScoped<IConsejoRepository, ConsejoRepository>(); //Vamos al controler para configurar esta parte. 
builder.Services.AddScoped<IFacultadRepository, FacultadRepository>();
builder.Services.AddScoped<IUserHelper, UserHelper>(); //Inyectamos la implementaci�n para el user

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

