using chatbot_ludo.Web.Data;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor
builder.Services.AddControllersWithViews();

// Configurar Entity Framework Core para usar SQL Server
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar SeedDB como servicio
builder.Services.AddTransient<SeedDB>(); //Esto es para registrar la inyeccion de la clase en la base de datos. 

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

app.UseAuthorization();

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

