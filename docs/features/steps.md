# Steps
FFlow is based on the concept of steps, which are individual units of work that can be executed in a workflow. Each step can perform a specific action, such as processing data, making API calls, or validating input.

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

