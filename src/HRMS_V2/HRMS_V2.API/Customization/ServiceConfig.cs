using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using HRMS_V2.Core.Configuration;
using HRMS_V2.Core.Entities;
using HRMS_V2.Core.Repositories.Base;
using HRMS_V2.Infrastructure.Data;
using HRMS_V2.Infrastructure.Repository.Base;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.Generation.Processors.Security;

namespace HRMS_V2.API.Customization;

public static class ServiceConfig
{
    public static IServiceCollection AddCustomAuthentication(this IServiceCollection services ,  IConfiguration configuration)
    {
        try
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration.GetRequiredSection("Tokens:Issuer").Value,
                        ValidAudience = configuration.GetRequiredSection("Tokens:Audience").Value,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetRequiredSection("Tokens:SecretKey").Value))
                    };
                });
            
            return services;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static IServiceCollection AddCustomConfiguration(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOptions();
        services.Configure<AdminSettings>(configuration);
        services.AddControllers()
            .ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Instance = context.HttpContext.Request.Path,
                        Status = StatusCodes.Status400BadRequest,
                        Detail = "Please refer to the errors property for additional details."
                    };

                    return new BadRequestObjectResult(problemDetails)
                    {
                        ContentTypes = { "application/problem+json", "application/problem+xml" }
                    };
                };
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.MaxDepth = 32;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        return services;
    }

    public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
    {
        services.AddOpenApiDocument(document =>
        {
            // configure OpenAPI document options
            document.DocumentName = "Authorization";
            document.Title = "My API v1";
            document.Description = "Type into the textbox: Bearer {your JWT token}.";
            document.Version = "v1";
            document.DocumentProcessors.Add(new SecurityDefinitionAppender("JWT Token",
                new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    Description = "Enter JWT Bearer token",
                    In = OpenApiSecurityApiKeyLocation.Header
                }));
            document.OperationProcessors.Add(new OperationSecurityScopeProcessor("JWT Token"));
        });
        return services;
    }

    public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = Environment.GetEnvironmentVariable("ConnectionString");
        services
            .AddEntityFrameworkSqlServer()
        .AddDbContext<AdminContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("HRMS_Database"),
                        sqlOptions =>
                        {
                            sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30),
                                errorNumbersToAdd: null);
                        }
                    ).UseLowerCaseNamingConvention(),
                ServiceLifetime.Scoped
            );

        return services;
    }

    public static IServiceCollection AddCustomIdentity(this IServiceCollection services)
    {
        var sp = services.BuildServiceProvider();
        using (var scope = sp.CreateScope())
        {
            var existingUserManager = scope.ServiceProvider.GetService<UserManager<AdminUser>>();

            if (existingUserManager == null)
            {
                services.AddIdentity<AdminUser, AdminRole>(
                        cfg => { cfg.User.RequireUniqueEmail = true; })
                    .AddEntityFrameworkStores<AdminContext>()
                    .AddDefaultTokenProviders();
            }
        }

        return services;
    }

    public static IServiceCollection AddCustomMvc(this IServiceCollection services)
    {
        services
            .AddMvc()
            //.AddFluentValidation(fv => { fv.RunDefaultMvcValidationAfterFluentValidationExecutes = false; })
            // .AddNewtonsoftJson(options =>
            // {
            //     options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            //     options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            // })
            .AddControllersAsServices();

        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy",
                builder => builder
                    .SetIsOriginAllowed((host) => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
        });

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services.AddAutoMapper(typeof(Program));
        
        var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(p => p.FullName != null
                                                                            && !p.FullName.StartsWith("Microsoft")
                                                                            && !p.FullName.StartsWith("System")).ToArray();
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssemblies(assemblies);
        });

        services.Configure<CookiePolicyOptions>(options =>
        {
            // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            options.CheckConsentNeeded = context => true;
            options.MinimumSameSitePolicy = SameSiteMode.None;
        });

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        
        return services;
    }
    
}