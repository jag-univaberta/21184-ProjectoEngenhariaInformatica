using System.Text;
using DocumentosApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Mapear a sec��o JwtSettings do appsettings.json para a classe JwtSettings
var jwtSettingsSection = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettings>(jwtSettingsSection);
var jwtSettings = jwtSettingsSection.Get<JwtSettings>(); // Obter valores para usar abaixo


// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ProjectoContext>(options =>
    options.UseNpgsql(connectionString, sqlOptions => // Passe a connection string aqui
    {
        sqlOptions.UseNetTopologySuite(); // Configure o NetTopologySuite aqui
    })
);
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddControllers();

// --- Configurar Autentica��o JWT ---
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; // Opcional mas comum
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true, // Validar a chave de assinatura
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)), // Obter a chave secreta

        ValidateIssuer = true, // Validar o emissor
        ValidIssuer = jwtSettings.Issuer, // Definir o emissor v�lido

        ValidateAudience = true, // Validar a audi�ncia
        ValidAudience = jwtSettings.Audience, // Definir a audi�ncia v�lida

        ValidateLifetime = true, // Validar o tempo de vida do token (n�o expirado)

        ClockSkew = TimeSpan.Zero // Toler�ncia para diferen�a de rel�gio entre servidor e cliente (Zero � mais seguro)
    };
    // Pode adicionar eventos aqui se precisar (ex: OnAuthenticationFailed, OnTokenValidated)
    /*
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context => {
            // Logar falha de autentica��o
            return Task.CompletedTask;
        },
        OnTokenValidated = context => {
            // Token validado, pode adicionar claims extras aqui se necess�rio
            return Task.CompletedTask;
        }
    };
    */
});

// -------------------------------------

builder.Services.AddAuthorization(); // Necess�rio para usar [Authorize]

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();


// ... (Swagger/OpenAPI - importante configurar para suportar Bearer token) ...
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Por favor, insira 'Bearer' [espa�o] e depois o seu token",
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement() {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});



var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}
 
app.UseHttpsRedirection();

// --- Adicionar Middleware de Autentica��o ---
// IMPORTANTE: UseAuthentication() deve vir ANTES de UseAuthorization()
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
