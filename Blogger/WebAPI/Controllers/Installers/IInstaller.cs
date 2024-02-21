namespace WebAPI.Controllers.Installers;

public interface IInstaller
{
    void InstallServices(IServiceCollection services, IConfiguration Configuration);
}
