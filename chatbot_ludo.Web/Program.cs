using System.Text.Json.Serialization;  // A�adir la referencia para el manejo de ciclos
using chatbot_ludo.Web.Data;
using chatbot_ludo.Web.Data.Entities;
using chatbot_ludo.Web.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        // Configurar el manejador de referencias c�clicas
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve; //Agremaos para evitar referencias ciclicas.
        options.JsonSerializerOptions.WriteIndented = true;  // Opcional, para hacer el JSON m�s legible
    });

// Configurar Entity Framework Core para usar SQL Server
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar SeedDB como servicio
builder.Services.AddTransient<SeedDB>();  // Inyecci�n de la clase para inicializar datos

// Configurar Identity con personalizaciones
builder.Services.AddIdentity<User, IdentityRole>(cfg =>
{
    cfg.User.RequireUniqueEmail = true;
    cfg.Password.RequireDigit = false;
    cfg.Password.RequiredUniqueChars = 0;
    cfg.Password.RequireLowercase = false;
    cfg.Password.RequireNonAlphanumeric = false;
    cfg.Password.RequireUppercase = false;
    cfg.Password.RequiredLength = 6;  // Longitud m�nima de la contrase�a
})
.AddEntityFrameworkStores<DataContext>()
.AddDefaultTokenProviders();

// Registrar Repository como un servicio Scoped
builder.Services.AddScoped<IConsejoRepository, ConsejoRepository>();
builder.Services.AddScoped<IFacultadRepository, FacultadRepository>();
builder.Services.AddScoped<IUserHelper, UserHelper>();  // Inyectar IUserHelper

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

app.UseAuthentication();  // Agregar el middleware para la autenticaci�n
app.UseAuthorization();   // Agregar el middleware para la autorizaci�n

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
            Console.WriteLine("Ocurri� un error al inicializar la base de datos: " + ex.Message);
        }
    }
}


