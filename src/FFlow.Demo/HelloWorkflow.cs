using FFlow.Core;
using FFlow.Extensions;
using FFlow.Observability.Listeners;
using FFlow.Observability.Metrics;

namespace FFlow.Demo;

public class HelloWorkflow : WorkflowDefinition
{
    private readonly MetricTrackingListener<InMemoryMetricsSink>? _metricTrackingListener;
    
    public HelloWorkflow(MetricTrackingListener<InMemoryMetricsSink>? metricTrackingListener = null)
    {
        _metricTrackingListener = metricTrackingListener;
        MetadataStore.SetName("Hello Workflow")
            .SetDescription("A simple workflow that greets the world and then says goodbye.");
    }

    public override void OnConfigure(WorkflowBuilderBase builder)
    {
        builder
            .StartWith<HelloStep>()
            .Input<HelloStep>((step, _) => step.Name = "Hello, World")
            .Delay(1000)
            .Then<GoodByeStep>()
            .Input<GoodByeStep>((step, _) => step.Name = "Goodbye, World");
    }

    public override Action<WorkflowOptions> OnConfigureOptions()
    {
        return options =>
        {
            if(_metricTrackingListener is not null)
                options.WithEventListener(_metricTrackingListener);
        };
    }
}