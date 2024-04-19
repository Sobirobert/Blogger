using NLog.Web;

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
        .UseNLog();
}