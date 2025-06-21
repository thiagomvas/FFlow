# Passing Data
You can pass data between steps in your workflow using the `IFlowContext` or manually setting the values. The context allows you to store and retrieve data as key-value pairs, enabling communication between different steps.

## Using the Context inside the step

Using `IFlowContext` is the recommended approach because it ensures seamless integration with other components and implementations within FFlow. This allows the library to manage data flow and state consistently and transparently across the entire workflow.

It has a few methods that allow you to set and get values, as well as manage outputs and inputs for each step.

### Using `SetValue` and `GetValue`
You can store data in the context using the `SetValue` method. This allows you to pass data from one step to another.

```csharp
public class StepOne : FlowStep
{
    public async Task ExecuteAsync(IFlowContext context)
    {
        // Store data in the context
        context.SetValue("message", "Hello from Step One");
        await Task.CompletedTask; // Simulate async work
    }
}
public class StepTwo : FlowStep
{
    public async Task ExecuteAsync(IFlowContext context)
    {
        // Retrieve data from the context
        var message = context.GetValue<string>("message");
        Console.WriteLine(message); // Output: Hello from Step One
        await Task.CompletedTask; // Simulate async work
    }
}
var workflow = new FFlowBuilder()
    .StartWith<StepOne>()
    .Then<StepTwo>()
    .Build();
```

### Using `SetSingleValue` and `GetSingleValue`

You can also set *singleton* values for each type of data, this allows you to retrieve the same value across different steps without needing to pass it explicitly and can be used when you want to share the same instance conveniently without worrying about keys. 

Internally, this is used for exception handling, as you can get the exception from the context in any step after it has been set with `context.GetSingleValue<Exception>()`.

```csharp
public class Step : FlowStep
{
    public async Task ExecuteAsync(IFlowContext context)
    {
        // Set a singleton value
        context.SetSingleValue(new MyData("John Doe", 42));
        await Task.CompletedTask; // Simulate async work
    }
}

public record MyData(string Name, int Age);
```

### Using `SetOutputFor`, `GetOutputFor` and `GetInputFor`

If your step produces data that can be considered an output, a good practice is to set the output for that step using the `SetOutputFor` method. This allows you to retrieve the output later in the workflow. 

Internally, `FlowStep` sets the input for the current step as the output of the previous step.

```csharp
public class StepWithOutput : FlowStep
{
    public async Task ExecuteAsync(IFlowContext context)
    {
        // Set output for this step
        var data = new MyData("Alice", 30);
        context.SetOutputFor<StepWithOutput, MyData>(data);
        await Task.CompletedTask; // Simulate async work
    }
}

public record MyData(string Name, int Age);
```

## Using `Input()` and `Output()`
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
