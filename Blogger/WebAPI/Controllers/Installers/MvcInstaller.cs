
using Application;
using Infrastructure;

namespace WebAPI.Controllers.Installers;

public class MvcInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddApplication();
        services.AddInfrastructure();

        services.AddControllers();
    }
}
