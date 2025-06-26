# Steps
FFlow is based on the concept of steps, which are individual units of work that can be executed in a workflow. Each step can perform a specific action, such as processing data, making API calls, or validating input.

## Custom Steps
You can create custom steps by implementing the `FlowStep` class or the `IFlowStep` interface. Custom steps allow you to encapsulate specific logic that can be reused across different workflows.

By implementing `FlowStep`, you also inherit additional functionality. It is heavily recommended to use `FlowStep` for custom steps, as it provides a lot of built-in features that make it easier to work with FFlow.

```csharp
public class CustomStep : FlowStep
{
    public override async Task ExecuteAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {
        // Custom logic here
        Console.WriteLine("Executing custom step...");
        
        // Simulate async work
        await Task.Delay(1000, cancellationToken);
        
        // Optionally set output for the next step
        context.SetOutput("result", "Custom step completed successfully.");
    }
}
```
---

## Flow Control
FFlow provides various flow control mechanisms to manage the execution of steps in a workflow. You can use conditional execution, loops, and parallel execution to control how steps are executed based on the context or other conditions.

### StartWith, Then and Finally
You can define the order of execution for your steps using the `StartWith`, `Then`, and `Finally` methods in the `FFlowBuilder`.
```csharp
var workflow = new FFlowBuilder()
    .StartWith<InitialStep>() // The first step in the workflow
    .Then<NextStep>() // The next step to execute
    .Finally<FinalStep>() // The final step to execute after all other steps
    .Build();
```

### Conditional Steps
You can use conditional steps to execute different steps based on a condition. The `If` method allows you to specify a condition and the steps to execute if the condition is true or false.
```csharp
var workflow = new FFlowBuilder()
    .StartWith<InitialStep>()
    .If<TrueStep, FalseStep>(context => context.Get<bool>("condition")) // Condition to check
    .Finally<FinalStep>()
    .Build();
```

There are also overloads that allow you to pass lambdas and `Func<IWorkflowBuilder>`s for more versatile behaviours:
```csharp
var workflow = new FFlowBuilder()
    .StartWith<InitialStep>()
    .If(context => context.Get<bool>("condition"),
        () => new FFlowBuilder().Then<TrueStep>(),
        () => new FFlowBuilder().Then<FalseStep>())
    .Finally<FinalStep>()
    .Build();
```

This does mean that you **need** to pass the `IServiceProvider` to the `FFlowBuilder` inside the delegates, otherwise the steps will not be resolved correctly if you use Dependency Injection.

### Switch Statement
You can use the `Switch` method to create a switch-like structure in your workflow. This allows you to execute different steps based on the value of a variable in the context.
```csharp
var workflow = new FFlowBuilder()
    .StartWith<InitialStep>()
    .Switch(b =>
    {
        b.Case<CaseOneStep>(ctx => ctx.Get<string>("name").StartsWith("J"));
        b.Case<CaseTwoStep>(ctx => ctx.Get<string>("name").StartsWith("A"));
    })
    .Finally<FinalStep>()
    .Build();
```

### Loops
You can use loops to iterate over a collection of items in your workflow. The `ForEach` method allows you to specify a collection and the steps to execute for each item in the collection.
```csharp
var workflow = new FFlowBuilder()
    .StartWith<InitialStep>()
    .ForEach(ctx => ctx.Get<string>("name").Split(' '),
        (ctx, ct) => Console.WriteLine(ctx.GetInput<string>()))
    .Finally<FinalStep>()
    .Build();
```

> [!NOTE]
> Currently, to consume the input in the loop, you **must** use `ctx.GetInput<T>()` to retrieve the current item being processed in the loop. 

### Parallel Execution
You can execute steps in parallel using the `Fork` method. This allows you to create branches in your workflow that can run concurrently.
```csharp
var workflow = new FFlowBuilder()
    .StartWith<InitialStep>()
    .Fork(ForkStrategy.FireAndForget,
        builder => builder...,
        builder => builder...) // Build parallel branches
    .Then<NextStep>()
    .Finally<FinalStep>()
    .Build();
```

### Continue with Workflow
You can use the `ContinueWith` method to continue with an `IWorkflowDefinition` rather than a specific step. This allows you to define a workflow that can be reused in multiple places.
```csharp
public class HelloWorkflow : IWorkflowDefinition
{
    public IWorkflow Build()
    {
        return new FFlowBuilder()
            .StartWith<HelloStep>()
            .Build();
    }
}

var workflow = new FFlowBuilder()
    .StartWith<InitialStep>()
    .ContinueWith<HelloWorkflow>() // Continue with the HelloWorkflow
    .Then<NextStep>()
    .Finally<FinalStep>()
    .Build();
```

### Delay
You can introduce a delay in your workflow using the `Delay` method. This allows you to pause the execution of the workflow for a specified duration.
```csharp
var workflow = new FFlowBuilder()
    .StartWith<InitialStep>()
    .Delay(TimeSpan.FromSeconds(5)) // Delay for 5 seconds
    .Then<NextStep>()
    .Finally<FinalStep>()
    .Build();
```

### Throw and ThrowIf
You can use the `Throw` and `ThrowIf` methods to throw exceptions in your workflow. This allows you to handle errors and control the flow of execution based on conditions.
```csharp
var workflow = new FFlowBuilder()
    .StartWith<InitialStep>()
    .ThrowIf(ctx => ctx.Get<bool>("condition"), "Condition is true!")
    .Then<NextStep>()
    .Finally<FinalStep>()
    .Build();
```

You may set the exception type by passing a generic type parameter to the `Throw` method:
```csharp
var workflow = new FFlowBuilder()
    .StartWith<InitialStep>()
    .Throw<InvalidOperationException>("An error occurred!")
    .Then<NextStep>()
    .Finally<FinalStep>()
    .Build();
```

## Extending steps with additional functionality
You can extend steps with additional functionality by implementing the respective interfaces. The builder and workflow classes will automatically use the implementations when applicable.

> [!NOTE]
> If you inherit from `FlowStep` when defining custom steps, all the interfaces below will already be implemented as virtual methods that you can then override.

### Retry Policies
You can create retry policies defined by `IRetryPolicy` and inject them into an `IRetryableStep` with `SetRetryPolicy`. This allows you to define how many times a step should be retried in case of failure, and under what conditions.

Here is an example of a custom retry policy that retries a step a fixed number of times with a delay between retries:

```csharp
public class FixedDelayRetryPolicy : IRetryPolicy
{
    private readonly int _maxRetries;
    private readonly TimeSpan _delay;
    public FixedDelayRetryPolicy(int maxRetries, TimeSpan delay)
    {
        _maxRetries = maxRetries;
        _delay = delay;
    }
    public async Task ExecuteAsync(Func<Task> action, CancellationToken cancellationToken = default)
    {
        for (int i = 0; i < _maxRetries; i++)
        {
            try
            {
                await action();
                return;
            }
            catch
            {
                if (i == _maxRetries - 1) throw;
                await Task.Delay(_delay, cancellationToken);
            }
        }
    }
}
```

> [!TIP]
> The RetryPolicies class provides a set of common retry policies that you can use out of the box, such as `RetryPolicies.FixedDelay` and `RetryPolicies.ExponentialBackoff`.

### Skippable Steps
Implementing `ISkippableStep` allows you to define steps that can be skipped based on certain conditions. This is useful for workflows where some steps may not be necessary depending on the context.

It is up to the developer on how and when, during the execution, the step should be skipped. `FlowStep` skips any and all operations other than setting the input for said step by default.

### Compensable Steps
Implementing `ICompensableStep` allows you to define steps that can be compensated if a previous step fails. This is useful for workflows where you need to roll back changes made by previous steps in case of an error.

For detailed information on compensable steps, refer to the [Compensation documentation](./compensation.md).
