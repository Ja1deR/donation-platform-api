using Microsoft.EntityFrameworkCore;
using Software_2.Data;
using Software_2.Repositories;
using Software_2.Services;

var builder = WebApplication.CreateBuilder(args);

// Configurar servicios
builder.Services.AddControllers();


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("cadena")));


var connectionString = builder.Configuration.GetConnectionString("cadena");

builder.Services.AddScoped<UsuariosRepository>(provider => new UsuariosRepository(connectionString));
builder.Services.AddScoped<UsuarioService>();

// Configurar Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configurar el pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();