using HRMS_V2.API.Application.Middlewares;
using HRMS_V2.API.Customization;
using HRMS_V2.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
//     .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true, reloadOnChange: true)
//     .AddEnvironmentVariables()
//     .Build();

// Add servics to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCustomMvc();
builder.Services.AddCustomDbContext(builder.Configuration);
builder.Services.AddCustomIdentity();
builder.Services.AddCustomSwagger();
builder.Services.AddCustomConfiguration(builder.Configuration);
builder.Services.AddCustomAuthentication(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AdminContext>();
        context.Database.Migrate();
        context.Database.EnsureCreated();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred seeding the DB.");
    }
}


// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("CorsPolicy");
app.UseOpenApi();
app.UseSwaggerUi3();
app.UseMiddleware<LoggingMiddleware>();
app.MapControllers();
app.Run();