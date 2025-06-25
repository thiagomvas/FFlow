# Workflow Definitions and Factory Pattern
To better use workflows when your use case isn't just simple scripts, FFlow provides a way to define workflows using the factory pattern through the `WorkflowDefinition` class. This allows you to create reusable workflow definitions that can be instantiated and run as needed.

## Defining a Workflow
To define a workflow, you need to create a class that inherits from `WorkflowDefinition`. In this class, you can configure the workflow steps and metadata.

The `OnConfigure` method is where you define the workflow steps using the `IWorkflowBuilder`.

The `OnConfigureOptions` method allows you to configure the `WorkflowOptions` instance for the workflows being produced.
```csharp
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
```

You can then register this workflow definition in your application startup or configuration code, or use `AddFlow()` from `FFlow.Extensions.Microsoft.DependencyInjection` to automatically register all definitions.

## Using the Workflow Definition
Once you have defined your workflow, you can instantiate it using the `Build()` method. This method creates a new instance of the workflow with the configured steps and options.

```csharp
var workflow = new ExampleWebhook(metricTrackingListener).Build();
await workflow.RunAsync(new WorkflowContext());
```
