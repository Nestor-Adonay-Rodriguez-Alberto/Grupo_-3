using API_RESTful.Models;
using API_RESTful.Servicios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// AGREGA LOS CONTROLADORES DE LA API:
builder.Services.AddControllers();

// DOCUMENTACION CON Swagger PARA TESTEAR:
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// INYECCION DE LA DB:
builder.Services.AddDbContext<MyDBcontext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Cadena_Conexion")));

// INYECCION DE LOS SERVICIOS PARA INTERACTUAR CON LA DB:
builder.Services.AddScoped<Servicios_Rol>();
builder.Services.AddScoped<Servicios_Empleado>();
builder.Services.AddScoped<Servicios_Producto>();
builder.Services.AddScoped<Servicios_De_Autenticacion>();

// AUTENTICACION CON JWT:
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

// ROLES DE AUTORIZACION:
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Ingeniero", policy => policy.RequireClaim("Rol", "Ingeniero"));
});
var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
