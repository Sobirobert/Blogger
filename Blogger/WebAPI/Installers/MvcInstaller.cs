using App.Metrics;
using Application;
using Application.Services;
using Application.Validators;
using FluentValidation.AspNetCore;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using WebAPI.MiddelWares;

namespace WebAPI.Installers;

public class MvcInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddApplication();
        services.AddInfrastructure();

        var metrics = AppMetrics.CreateDefaultBuilder().Build();
        services.AddMetrics(metrics);

        services.AddMemoryCache();

        services.AddControllers()
            .AddFluentValidation(options =>
            {
                options.RegisterValidatorsFromAssemblyContaining<CreatePostDtoValidator>();
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.WriteIndented = true;
            })
            .AddXmlSerializerFormatters();

        services.AddAuthorization();

        services.AddTransient<UserResolverService>();
        services.AddScoped<ErrorHandlingMiddelware>();

        services.AddApiVersioning(x =>
        {
            x.DefaultApiVersion = new ApiVersion(1, 0);
            x.AssumeDefaultVersionWhenUnspecified = true;
            x.ReportApiVersions = true;
            x.ApiVersionReader = new HeaderApiVersionReader("x-api-version");
        });
    }
}