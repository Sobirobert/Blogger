using NLog.Web;
using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace WebAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

        try
        {
            //throw new Exception("Fatal error!");
            CreateHostBuilder(args).Build().Run();
        }
        catch (Exception ex)
        {
            logger.Fatal(ex, "API stopped");
            //Environment.FailFast("API stopped because of exception", ex);
            throw;
        }
        //finally
        //{
        //    //logger.Info("Application is shutting down.");
        //    //NLog.LogManager.Flush();
        //    // NLog.LogManager.Shutdown();
        //}
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            })
        //.UseNLog();
           .UseSerilog((context, configuration) => 
           {
            configuration.Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(context.Configuration["ElasticConfiguration:Uri"]))
            {
                IndexFormat = $"{context.Configuration["ApplicationName"]}-logs-{context.HostingEnvironment.EnvironmentName?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}",
                AutoRegisterTemplate = true,
                NumberOfShards = 2,
                NumberOfReplicas = 1
            })
            .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
            .ReadFrom.Configuration(context.Configuration);
           });
}