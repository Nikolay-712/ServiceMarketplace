using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using ServiceMarketplace.Infrastructure.Configurations;
using ServiceMarketplace.Infrastructure.Filters;
using System.Globalization;

internal class Program
{
    [Obsolete]
    private static void Main(string[] args)
    {
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
            //options.RegisterValidatorsFromAssembly();
            options.LocalizationEnabled = true;
        });

        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        SwaggerConfiguration(services, configuration);
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
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

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
    }
}