using API.Extension;
using AspNetCoreRateLimit;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(Assembly.GetEntryAssembly());

builder.Services.ConfigureRateLimition();
// Add services to the container.
builder.Services.ConfigureCors();
builder.Services.AddAplicationServices();
builder.Services.ConfigureApiVersioning();
builder.Services.AddControllers(options =>
{
    options.RespectBrowserAcceptHeader = true;
    options.ReturnHttpNotAcceptable = true;
}).AddXmlSerializerFormatters();

builder.Services.AddDbContext<InventarioContext>(options => {
    try
    {
        var serverVersion = new MySqlServerVersion(new Version(8, 0, 28));
        options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), serverVersion);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex);
    }

});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseIpRateLimiting();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();
    try
    {
        var context = services.GetRequiredService<InventarioContext>();
        await context.Database.MigrateAsync();
    }
    catch (Exception ex)
    {
        var logger = loggerFactory.CreateLogger<Program>();
        logger.LogError(ex, "Ocurrio un error durante la migraci�n");
    }
}

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
