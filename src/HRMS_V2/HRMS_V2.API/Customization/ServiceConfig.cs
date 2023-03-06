using System.Text;
using System.Text.Json.Serialization;
using HRMS_V2.Core.Configuration;
using HRMS_V2.Core.Entities;
using HRMS_V2.Core.Repositories.Base;
using HRMS_V2.Infrastructure.Authorization;
using HRMS_V2.Infrastructure.Data;
using HRMS_V2.Infrastructure.Repository.Base;
using HRMS_V2.Infrastructure.Securiy;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.Generation.Processors.Security;

namespace HRMS_V2.API.Customization;

public static class ServiceConfig
{
    public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        try
        {
            var securityTokens = configuration.GetSection("Tokens").Get<Tokens>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = securityTokens.Issuer,
                        ValidAudience = securityTokens.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityTokens.Key))
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
            document.Title = "HRMS API";
            document.Version = "v1";

            // Configure JWT authentication for the OpenAPI document generator
            document.AddSecurity("bearer", Enumerable.Empty<string>(), new OpenApiSecurityScheme
            {
                Type = OpenApiSecuritySchemeType.Http,
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = OpenApiSecurityApiKeyLocation.Header,
                Scheme = "bearer",
            });

            document.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("bearer"));
        });

        return services;
    }

    public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
    {
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

        services.AddMediatR(assemblies);

        services.Configure<CookiePolicyOptions>(options =>
        {
            // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            options.CheckConsentNeeded = context => true;
            options.MinimumSameSitePolicy = SameSiteMode.None;
        });

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        return services;
    }

    public static IServiceCollection SeedDatabase(this IServiceCollection services)
    {
        var sp = services.BuildServiceProvider();
        using (var scope = sp.CreateScope())
        {
            var dbService = scope.ServiceProvider;
            try
            {
                var context = dbService.GetRequiredService<AdminContext>();
                context.Database.Migrate();
                context.Database.EnsureCreated();
            }
            catch (Exception ex)
            {
                var logger = dbService.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred seeding the DB.");
            }
        }

        return services;
    }

    public static IServiceCollection AddSecurity(this IServiceCollection services)
    {
        services
              .AddMvcCore()
              .AddAuthorization(options =>
              {                  
                  options.AddPolicy(AuthorizationPolicies.ReadHolidayRequirement, policyBuilder => policyBuilder.AddRequirements(new ReadHolidayRequirement()));
                  options.AddPolicy(AuthorizationPolicies.ManageHolidayRequirement, policyBuilder => policyBuilder.AddRequirements(new ManageHolidaysRequirement()));                  
              });

        services.AddSingleton<IAuthorizationHandler, ManageHolidaysRequirementHandler>();
        services.AddSingleton<IAuthorizationHandler, ReadHolidaysRequirementHandler>();

        return services;
    }
}