using App.Metrics;
using App.Metrics.Counter;

namespace WebAPI.Metrics;

public class MetricsRegistry
{
    public static CounterOptions CreatedPostsCounter => new()
    {
        Name = "Created Posts",
        Context = "bloggerapi",
        MeasurementUnit = Unit.Calls
    };
}
