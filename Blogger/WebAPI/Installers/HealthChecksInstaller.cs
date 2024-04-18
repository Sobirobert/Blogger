using Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using WebAPI.HealthChecks;

namespace WebAPI.Installers;

public class HealthChecksInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration Configuration)
    {
        services.AddHealthChecks()
            .AddDbContextCheck<BloggerContext>("Database");

        services.AddHealthChecks()
                .AddCheck<ResponsTimeHealthCheck>("Network speed test");

        services.AddHealthChecksUI()
            .AddInMemoryStorage();

    }
}
