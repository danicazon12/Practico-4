using ApiInicial.Core;
using ApiInicial.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var conexionLocal = builder.Configuration.GetConnectionString("CatalogoConection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(conexionLocal));

// Se registran las interfaces, no las clases concretas
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();

builder.Services.AddProblemDetails();

// Permite que el frontend de Angular (puerto 4200) consuma el API
builder.Services.AddCors(options =>
{
    options.AddPolicy("Angular", policy =>
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    // Aplica las migraciones pendientes al iniciar en desarrollo
    using var scope = app.Services.CreateScope();
    scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.Migrate();
}
else
{
    app.UseExceptionHandler();
    app.UseHttpsRedirection();
}

app.UseCors("Angular");

app.UseAuthorization();

app.MapControllers();

app.Run();
