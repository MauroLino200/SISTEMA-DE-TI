using API_GestionEmpleados.Helpers;
using API_GestionEmpleados.Repositories;
using API_GestionEmpleados.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IDatabaseExecutor>(provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    return new DatabaseExecutor(config.GetConnectionString("Default"));
});

builder.Services.AddScoped<IEmpleadoRepository, EmpleadoRepository>();
builder.Services.AddScoped<IUsuariosRepository, UsuarioRepository>();
builder.Services.AddScoped<IEvaluacionRepository, EvaluacionRepository>();
builder.Services.AddScoped<IEquipoTrabajoRepository, EquipoTrabajoRepository>();
builder.Services.AddScoped<IReservaRepository, ReservaRepository>();
builder.Services.AddScoped<IDevolucionesRepository, DevolucionesRepository>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
