using FFlow.Core;
using FFlow.Extensions;
using FFlow.Observability.Listeners;
using FFlow.Observability.Metrics;

namespace FFlow.Samples.BasicServer.Workflows;

public class ExampleWebhook : WorkflowDefinition
{
    private readonly MetricTrackingListener<InMemoryMetricsSink> _metricTrackingListener;

    public ExampleWebhook(MetricTrackingListener<InMemoryMetricsSink> metricTrackingListener)
    {
        _metricTrackingListener = metricTrackingListener;
        MetadataStore.SetName("Example Webhook")
            .SetDescription("This is an example webhook workflow definition.");
    }

    public override void OnConfigure(IWorkflowBuilder builder)
    {
        builder.StartWith((ctx, _) =>
            {
                ctx.SetValue("message", "Webhook received successfully.");
            })
            .Then((ctx, _) =>
            {
                var message = ctx.GetValue<string>("message");
            });
    }
    
    public override Action<WorkflowOptions> OnConfigureOptions()
    {
        return options =>
        {
            options.WithEventListener(_metricTrackingListener);
        };
    }
}