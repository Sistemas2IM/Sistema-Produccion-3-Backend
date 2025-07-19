using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Sistema_Produccion_3_Backend.ApiKey;
using Sistema_Produccion_3_Backend.AutomapperProfiles;
using Sistema_Produccion_3_Backend.Models;
using Sistema_Produccion_3_Backend.Services;
using Sistema_Produccion_3_Backend.Services.RequestLock;
using Sistema_Produccion_3_Backend.Validators.Auth;
using Sistema_Produccion_3_Backend.Validators.ProductoTerminado;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configuración de la base de datos
builder.Services.AddDbContext<base_nuevaContext>(options =>
    options.UseSqlServer("name=ConnectionStrings:Prod3Connection"));

builder.Services.AddScoped<base_nuevaContextProcedures>();

// Documentación sobre Swagger/OpenAPI https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuración de HttpClient para llamadas HTTP
builder.Services.AddHttpClient();

// Automapper - uso de DTO
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddAutoMapper(typeof(Program));

// API KEY de seguridad ------------------------------------------------------------
builder.Services.AddTransient<IApiKeyValidation, ApiKeyValidation>();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidarApiEndpoint>(); // Registrar el filtro globalmente
});

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("apiKey", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "apiKey",
        Type = SecuritySchemeType.ApiKey,
        Description = "API Key needed to access the endpoints"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "apiKey"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddSingleton<IRequestLockService, RequestLockService>();

//---------------------------------------------------------------------------------

// Servicios de Autenticación de Usuarios
builder.Services.AddScoped<IAuthService, AuthService>();

// Configuración de comportamiento de errores de validación
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
                            .Where(e => e.Value.Errors.Count > 0)
                            .Select(e => new
                            {
                                field = e.Key,
                                error = e.Value.Errors.First().ErrorMessage
                            })
                            .ToArray();
        return new BadRequestObjectResult(new { message = "Errores de validación", errors });
    };
});

// Definir los validadores
builder.Services.AddValidatorsFromAssemblyContaining<RegisterValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<PTerminadoValidator>();
builder.Services.AddFluentValidationAutoValidation();

// Habilitar Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin();
            builder.AllowAnyHeader();
            builder.AllowAnyMethod();
        });
});

// Configuración de JWT
var key = builder.Configuration.GetValue<string>("JWTSetting:securitykey");
var keyBytes = Encoding.ASCII.GetBytes(key);

builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(config =>
{
    config.RequireHttpsMetadata = false;
    config.SaveToken = true;
    config.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseCors("AllowAllOrigins");

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication(); // Middleware de autenticación JWT
app.UseAuthorization();  // Middleware de autorización

app.MapControllers();

app.Run();
