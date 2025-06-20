# Passing Data
You can pass data between steps in your workflow using the `IFlowContext` or manually setting the values. The context allows you to store and retrieve data as key-value pairs, enabling communication between different steps.

## Using the Context inside the step
You can store data in the context using the `Set` method. This allows you to pass data from one step to another.

```csharp
public class StepOne : FlowStep
{
    public async Task ExecuteAsync(IFlowContext context)
    {
        // Store data in the context
        context.Set("message", "Hello from Step One");
        await Task.CompletedTask; // Simulate async work
    }
}
public class StepTwo : FlowStep
{
    public async Task ExecuteAsync(IFlowContext context)
    {
        // Retrieve data from the context
        var message = context.Get<string>("message");
        Console.WriteLine(message); // Output: Hello from Step One
        await Task.CompletedTask; // Simulate async work
    }
}
var workflow = new FFlowBuilder()
    .StartWith<StepOne>()
    .Then<StepTwo>()
    .Build();
```

## Using `Input()` and overloads
You can also use the `Input` method to pass data directly to a step. This is useful when you want to provide specific input values without relying on the context. It allows you to directly set the values for the properties of the step.

```csharp
public class StepWithInput : FlowStep
{
    public string InputMessage { get; set; }
    public int AnotherProperty { get; set; }

    public async Task ExecuteAsync(IFlowContext context)
    {
        Console.WriteLine(InputMessage); // Output: Hello from Input
        await Task.CompletedTask; // Simulate async work
    }
}

var workflow = new FFlowBuilder()
    .StartWith<StepWithInput>()
    .Input(step => step.InputMessage, "Hello from Input")
    .Build();
```

There are overloads that allow you to also get the values directly from the context.

```csharp
var workflow = new FFlowBuilder()
    .StartWith<StepWithInput>()
    .Input(step => step.InputMessage, context => context.Get<string>("message"))
    .Build();
```

You can also use the `Input` method to set multiple properties at once:

```csharp
var workflow = new FFlowBuilder()
    .StartWith<StepWithInput>()
    .Input(step => {
        step.InputMessage = "Hello, FFlow!";
        step.AnotherProperty = 1234;
    })
    .Build();
```

All of these methods allow chaining, so you can set multiple inputs in a single fluent call.

## Using `Get/SetInput()`
You can also use the `GetInput` and `SetInput` methods to manage inputs more explicitly. This is particularly useful when **you are certain** that the previous step will correctly set the input value you are trying to retrieve.
```csharp
public class StepWithDynamicInput : FlowStep
{
    public async Task ExecuteAsync(IFlowContext context)
    {
        var input = context.GetInput<string>();
        Console.WriteLine(input);
        await Task.CompletedTask; 
    }
}