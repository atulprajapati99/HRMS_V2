using Autofac;
using Autofac.Extensions.DependencyInjection;
using HRMS_V2.API.Application.Middlewares;
using HRMS_V2.API.Customization;
using HRMS_V2.API.Ioc;
using HRMS_V2.Application.IOC;
using HRMS_V2.Infrastructure.IOC;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCustomMvc();
builder.Services.AddCustomDbContext(builder.Configuration);
builder.Services.AddCustomIdentity();
builder.Services.AddCustomSwagger();
builder.Services.AddSecurity();
builder.Services.AddCustomConfiguration(builder.Configuration);
builder.Services.AddCustomAuthentication(builder.Configuration);
builder.Services.SeedDatabase();

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new MediatorModule()));
builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new InfrastructureModule()));
builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new ApplicationModule()));

var app = builder.Build();

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