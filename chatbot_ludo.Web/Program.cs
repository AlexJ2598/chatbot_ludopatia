using chatbot_ludo.Web.Data;
using chatbot_ludo.Web.Data.Entities;
using chatbot_ludo.Web.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = null;  // Eliminamos referencias cíclicas al usar DTO
        options.JsonSerializerOptions.WriteIndented = true;     // Opcional, para hacer el JSON más legible
    });

// Configurar Entity Framework Core para usar SQL Server
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar SeedDB como servicio transitorio para inicializar datos
builder.Services.AddTransient<SeedDB>();

// Configurar Identity con personalizaciones para usuario y roles
builder.Services.AddIdentity<User, IdentityRole>(cfg =>
{
    cfg.User.RequireUniqueEmail = false;   // No requiere email único
    cfg.Password.RequireDigit = false;     // No requiere dígito en la contraseña
    cfg.Password.RequiredUniqueChars = 0;  // No requiere caracteres únicos
    cfg.Password.RequireLowercase = false; // No requiere minúsculas
    cfg.Password.RequireNonAlphanumeric = false;  // No requiere caracteres especiales
    cfg.Password.RequireUppercase = false;  // No requiere mayúsculas
    cfg.Password.RequiredLength = 6;       // Longitud mínima de la contraseña
})
.AddEntityFrameworkStores<DataContext>()
.AddDefaultTokenProviders();  // Agregar soporte para generación de tokens

// Configuración de autenticación por cookies (para la web) y JWT (para las APIs)
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    // Configuración de cookies
    options.Cookie.SameSite = SameSiteMode.Lax;  // 'Lax' debería ser lo suficientemente permisivo en la mayoría de los casos.
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;  // Solo enviar cookies seguras en HTTPS
    options.LoginPath = "/Account/Login";  // Ruta para redirigir al login
    options.ExpireTimeSpan = TimeSpan.FromDays(15);  // Duración de la cookie de sesión
    options.SlidingExpiration = true;  // Renovar la cookie en cada solicitud
})
.AddJwtBearer(cfg =>
{
    // Configuración de validación del token JWT para las APIs
    cfg.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Tokens:Issuer"],
        ValidAudience = builder.Configuration["Tokens:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Tokens:Key"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});

// Registrar repositorios como servicios Scoped (durante la vida de una solicitud)
builder.Services.AddScoped<IConsejoRepository, ConsejoRepository>();
builder.Services.AddScoped<IFacultadRepository, FacultadRepository>();
builder.Services.AddScoped<IUserHelper, UserHelper>();  // Inyectar IUserHelper

var app = builder.Build();

// Llamar a SeedAsync para inicializar datos
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

app.UseAuthentication();  // Habilitar el middleware de autenticación para manejar cookies y JWT
app.UseAuthorization();   // Habilitar el middleware de autorización para proteger rutas basadas en roles o claims

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
            seedDB.SeedAsync().Wait();  // Ejecutar la inicialización de la base de datos
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ocurrió un error al inicializar la base de datos: " + ex.Message);
        }
    }
}
