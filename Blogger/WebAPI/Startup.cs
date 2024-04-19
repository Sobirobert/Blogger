using Application.Dto;
using HealthChecks.UI.Client;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.OData.Edm;
using WebAPI.Installers;
using WebAPI.MiddelWares;

namespace WebAPI;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.InstallServicesInAssembly(Configuration);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPI v1"));
        }

        app.UseMiddleware<ErrorHandlingMiddelware>();

        //app.UseHealthChecks("/health", new HealthCheckOptions
        //{
        //    ResponseWriter = async (contex, report) =>
        //    {
        //        contex.Response.ContentType = "application/json";

        //        var response = new HealthCheckResponse
        //        {
        //            Status = report.Status.ToString(),
        //            Checks = report.Entries.Select(x => new HealthCheck
        //            {
        //                Component = x.Key,
        //                Status = x.Value.Status.ToString(),
        //                Description = x.Value.Description
        //            }),
        //            Duration = report.TotalDuration
        //        };
        //        await contex.Response.WriteAsync(JsonConvert.SerializeObject(response));
        //    }
        //});

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthentication();  // do logowania się na konto

        app.UseAuthorization();   // do logowania się na konto

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHealthChecks("/health", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
            endpoints.MapHealthChecksUI();
        });
    }

    public static IEdmModel GetEdmModel()
    {
        var builder = new ODataConventionModelBuilder();
        builder.EntitySet<PostDto>("Posts");
        return builder.GetEdmModel();
    }

    //LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
}