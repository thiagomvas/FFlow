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

    public override void OnConfigure(IWorkflowBuilder builder)
    {
        builder
            .StartWith<HelloStep>()
            .Input<HelloStep, string>(step => step.Name, "World")
            .Delay(1000)
            .Then<GoodByeStep>()
            .Input<GoodByeStep, string>(step => step.Name, "World");
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