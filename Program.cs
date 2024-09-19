using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend;
using Sistema_Produccion_3_Backend.Models;
using Sistema_Produccion_3_Backend.AutomapperProfiles;
using Sistema_Produccion_3_Backend.Services;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Sistema_Produccion_3_Backend.ApiKey;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<base_nuevaContext>(options =>
        options.UseSqlServer("name=ConnectionStrings:Prod3Connection"));

// Documentacion sobre Swagger/OpenAPI https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
//---------------------------------------------------------------------------------

//Servicos de Autenticacion de Usuarios
builder.Services.AddScoped<IAuthService, AuthService>();

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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
