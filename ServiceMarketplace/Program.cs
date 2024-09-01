using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ServiceMarketplace.Data;
using ServiceMarketplace.Data.Configurations;
using ServiceMarketplace.Data.Entities;
using ServiceMarketplace.Infrastructure.Configurations;
using ServiceMarketplace.Infrastructure.Filters;
using System.Globalization;
using System.Text;
using ServiceMarketplace.Services.Interfaces.Users;
using ServiceMarketplace.Services.Implementations.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ServiceMarketplace.Services.Interfaces.Identity;
using ServiceMarketplace.Services.Implementations.Identity;

using AdministrationInterfaces = ServiceMarketplace.Services.Interfaces.Administration;
using AdministrationImplementations = ServiceMarketplace.Services.Implementations.Administration;
using OwnerInterfaces = ServiceMarketplace.Services.Interfaces.Owner;
using OwnerImplementations = ServiceMarketplace.Services.Implementations.Owner;
using ServiceMarketplace.Infrastructure.Middleware;
using ServiceMarketplace.Models.Validators;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;

internal class Program
{
    [Obsolete]
    private static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        ConfigureAppSettings(builder.Host, builder.Environment);
        AddAppSettings(builder.Services, builder.Configuration);

        ConfigureServices(builder.Services, builder.Configuration);

        WebApplication app = builder.Build();

        ConfigureRequestLocalization(app, builder.Configuration);
        ConfigureRequestPipeline(app);
        app.Run();
    }

    private static void ConfigureAppSettings(IHostBuilder hostBuilder, IHostEnvironment environment)
    {
        hostBuilder.ConfigureAppConfiguration(config =>
        {
            config
                .SetBasePath(environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
        });
    }

    private static void AddAppSettings(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RequestLocalizationSettings>(configuration.GetSection(nameof(RequestLocalizationSettings)));
        services.Configure<SwaggerSettings>(configuration.GetSection(nameof(SwaggerSettings)));
        services.Configure<JwtTokenSettings>(configuration.GetSection(nameof(JwtTokenSettings)));
        services.Configure<CorsSettings>(configuration.GetSection(nameof(CorsSettings)));
    }

    [Obsolete]
    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers(options =>
        {
            options.Filters.Add<ModelStateFilter>();
            options.Filters.Add<ExceptionFilter>();

        });

        services.AddFluentValidation(options =>
        {
            options.DisableDataAnnotationsValidation = true;
            options.RegisterValidatorsFromAssemblyContaining<ManageCategoryValidator>();
            options.LocalizationEnabled = true;
        });

        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        CorsSettings corsSettings = new();
        configuration.GetSection(nameof(CorsSettings)).Bind(corsSettings);
        services.AddCors(options =>
        {
            options.AddPolicy(corsSettings.PolicyName,
                builder =>
                {
                    builder.WithOrigins(corsSettings.Origins)
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
        });

        SwaggerConfiguration(services, configuration);
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        ApplicationContextConfiguration(services, configuration);
        JwtTokenConfiguration(services, configuration);

        //Identity area
        services.AddScoped<ITokenManager, TokenManager>();
        services.AddScoped<IAccountService, AccountService>();

        //Administration area
        services.AddScoped<AdministrationInterfaces.ICategoryService, AdministrationImplementations.CategoryService>();
        services.AddScoped<AdministrationInterfaces.ICityService, AdministrationImplementations.CityService>();
        services.AddScoped<AdministrationInterfaces.IRoleService, AdministrationImplementations.RoleService>();
        services.AddScoped<AdministrationInterfaces.IUserService, AdministrationImplementations.UserService>();

        //Owner area
        services.AddScoped<OwnerInterfaces.IServiceService, OwnerImplementations.ServiceService>();
        services.AddScoped<OwnerInterfaces.ICategoryService, OwnerImplementations.CategoryService>();
        services.AddScoped<OwnerInterfaces.ICityService, OwnerImplementations.CityService>();
        services.AddScoped<OwnerInterfaces.IContactService, OwnerImplementations.ContactService>();
        services.AddScoped<OwnerInterfaces.IRatingService, OwnerImplementations.RatingService>();
        services.AddScoped<OwnerInterfaces.IBusinessHoursService, OwnerImplementations.BusinessHoursService>();

        //User area
        services.AddScoped<IRatingService, RatingService>();
        services.AddScoped<IServiceService, ServiceService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ICityService, CityService>();

    }

    private static void ConfigureRequestLocalization(IApplicationBuilder app, IConfiguration configuration)
    {
        RequestLocalizationSettings requestLocalization = new();
        configuration.GetSection(nameof(RequestLocalizationSettings)).Bind(requestLocalization);

        IList<CultureInfo> supportedCultures = new List<CultureInfo>();
        requestLocalization.SupportedCultures.ToList()
            .ForEach(x => supportedCultures.Add(new CultureInfo(x)));

        app.UseRequestLocalization(new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture(requestLocalization.DefaultRequestCulture),
            SupportedCultures = supportedCultures,
            SupportedUICultures = supportedCultures
        });
    }

    private static void SwaggerConfiguration(IServiceCollection services, IConfiguration configuration)
    {
        SwaggerSettings swaggerSettings = new();
        configuration.GetSection(nameof(SwaggerSettings)).Bind(swaggerSettings);

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc(swaggerSettings.Version, new OpenApiInfo
            {
                Title = swaggerSettings.Title,
                Version = swaggerSettings.Version,
            });

            //string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            //string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            //options.IncludeXmlComments(xmlPath);

            options.AddSecurityDefinition(swaggerSettings.SecurityDefinitionType, new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = swaggerSettings.Description,
                Name = swaggerSettings.SecurityDefinitionName,
                Type = SecuritySchemeType.Http,
                BearerFormat = swaggerSettings.BearerFormat,
                Scheme = swaggerSettings.Scheme,
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type= ReferenceType.SecurityScheme,
                            Id= swaggerSettings.SecurityDefinitionType,
                        }
                    },
                    new string[]{}
                }
            });
        });

        services.AddFluentValidationRulesToSwagger();
    }

    private static void ConfigureRequestPipeline(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        CorsSettings corsSettings = new();
        app.Configuration.GetSection(nameof(CorsSettings)).Bind(corsSettings);
        app.UseCors(corsSettings.PolicyName);

        app.UseHttpsRedirection();
        //app.UseMiddleware<TokenValidatorMiddleware>();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
    }

    private static void ApplicationContextConfiguration(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationContext>(options => options
            .UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services
            .AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationContext>();
    }

    private static void JwtTokenConfiguration(IServiceCollection services, IConfiguration configuration)
    {
        JwtTokenSettings jwtTokenSettings = new();
        configuration.GetSection(nameof(JwtTokenSettings)).Bind(jwtTokenSettings);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtTokenSettings.Issuer,
                ValidAudience = jwtTokenSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtTokenSettings.Key))
            };
        });
    }
}