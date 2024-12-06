using EMGVoitures.Data;
using EMGVoitures.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EMGVoitures.Models;

var builder = WebApplication.CreateBuilder(args);

// Ajouter les services au conteneur.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<JwtService>(); // Service JWT

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Ajouter la configuration CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", builder =>
    {
        builder.WithOrigins("http://localhost:4200")  // L'origine de votre front-end
               .AllowAnyMethod()  // Autoriser toutes les m�thodes HTTP (GET, POST, etc.)
               .AllowAnyHeader()  // Autoriser tous les en-t�tes
               .AllowCredentials();  // Permettre les cookies si n�cessaire
    });
});

// Configurer l'authentification JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Bearer";
    options.DefaultChallengeScheme = "Bearer";
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

var app = builder.Build();

// Cr�er un utilisateur par d�faut si aucun utilisateur n'existe
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    // Cr�er le r�le "Admin" s'il n'existe pas
    var roleExist = await roleManager.RoleExistsAsync("Admin");
    if (!roleExist)
    {
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    }

    // Cr�er un utilisateur admin par d�faut si aucun utilisateur n'existe
    var user = await userManager.FindByNameAsync("admin");

    if (user == null)
    {
        user = new ApplicationUser
        {
            UserName = "admin",
            Email = "admin@example.com"
        };
        var result = await userManager.CreateAsync(user, "Admin123!");  // Assurez-vous que le mot de passe respecte les r�gles

        if (result.Succeeded)
        {
            // Ajouter l'utilisateur au r�le Admin
            await userManager.AddToRoleAsync(user, "Admin");
        }
    }
}

app.UseSwagger();
app.UseSwaggerUI();

// Activer CORS dans le pipeline
app.UseCors("AllowLocalhost");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
