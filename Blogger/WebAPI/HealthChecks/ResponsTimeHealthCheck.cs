using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace WebAPI.HealthChecks;

public class ResponsTimeHealthCheck : IHealthCheck
{
    private Random rnd = new Random();

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        int responseTimeMS = rnd.Next(1, 100);

        if (responseTimeMS < 100)
        {
            return Task.FromResult(HealthCheckResult.Healthy($"The response time looks good ({responseTimeMS})"));
        }
        else if (responseTimeMS < 200)
        {
            return Task.FromResult(HealthCheckResult.Degraded($"The response time is a bit slow ({responseTimeMS})"));
        }
        else
        {
            return Task.FromResult(HealthCheckResult.Unhealthy($"The response time is unacceptable ({responseTimeMS})"));
        }
    }
}