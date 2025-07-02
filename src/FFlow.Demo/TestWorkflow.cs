using FFlow.Core;
using FFlow.Observability.Listeners;
using FFlow.Observability.Metrics;

namespace FFlow.Demo;

public class TestWorkflow : WorkflowDefinition
{
    private readonly MetricTrackingListener<InMemoryMetricsSink> _metrics;

    public TestWorkflow(MetricTrackingListener<InMemoryMetricsSink> metrics)
    {
        _metrics = metrics;
    }
    public override void OnConfigure(IWorkflowBuilder builder)
    {
        builder
            .StartWith((ctx, _) => ctx.SetValue("now", DateTimeOffset.UtcNow))
            .Then((ctx, _) => Console.WriteLine($"TestWorkflow: {ctx.GetValue<DateTimeOffset>("now")}"));   
    }

    public override Action<WorkflowOptions> OnConfigureOptions()
    {
        return options =>
        {
            options.WithEventListener(_metrics);
        };
    }
}