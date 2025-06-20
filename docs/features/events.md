# Event Handling
FFlow provides an event listener interface for handling events within the workflow. This allows you to respond to various lifecycle events of the workflow execution, such as when a step starts or completes.

You can implement the `IFlowEventListener` interface to create custom event listeners. It can be used to log events, trigger additional actions, or modify the workflow context based on specific events.

## Implementing an Event Listener
To create a custom event listener, implement the `IFlowEventListener` interface:

```csharp
public class ConsoleFlowEventListener : IFlowEventListener
{
    public void OnWorkflowStarted(IWorkflow workflow)
    {
        Console.WriteLine($"[Workflow Started] Time={DateTime.UtcNow:o}");
    }
    public void OnWorkflowCompleted(IWorkflow workflow)
    {
        Console.WriteLine($"[Workflow Completed] Time={DateTime.UtcNow:o}");
    }
    public void OnWorkflowFailed(IWorkflow workflow, Exception exception)
    {
        Console.WriteLine($"[Workflow Failed] Time={DateTime.UtcNow:o} Exception={exception}");
    }
    public void OnStepStarted(IFlowStep step, IFlowContext context)
    {
        Console.WriteLine($"  [Step Started] Step={step.GetType().Name} Time={DateTime.UtcNow:o}");
    }
    public void OnStepCompleted(IFlowStep step, IFlowContext context)
    {
        Console.WriteLine($"  [Step Completed] Step={step.GetType().Name} Time={DateTime.UtcNow:o}");
    }
    public void OnStepFailed(IFlowStep step, IFlowContext context, Exception exception)
    {
        Console.WriteLine($"  [Step Failed] Step={step.GetType().Name} Time={DateTime.UtcNow:o} Exception={exception}");
    }
}
```

## Registering the Event Listener
To use a custom event listener, you need to register it inside a `WorkflowOptions` instance and pass it to the builder:

```csharp
var options = new WorkflowOptions
{
    EventListener = new ConsoleFlowEventListener()
};

var builder = new FFlowBuilder()
    .WithOptions(options)
    .StartWith<SomeStep>()
    .Build();
```
> [!TIP]
> You can use multiple event listeners by creating a composite listener that aggregates multiple implementations of `IFlowEventListener` or by calling `WithEventListener`.

The event listener will then be invoked at the appropriate times during the workflow execution, allowing you to monitor and respond to the workflow's lifecycle events.

