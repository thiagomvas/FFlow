# Passing Data
You can pass data between steps in your workflow using the `IFlowContext` or manually setting the values. The context allows you to store and retrieve data as key-value pairs, enabling communication between different steps.

## Configuring which context to use
There are a few ways to configure the context that will be used in your workflow. 

### Using `UseContext(IFlowContext context)`
You can pass an instance of `IFlowContext` to the `UseContext` method when building your workflow. This allows you to use a custom context implementation or a specific instance of a context.

```csharp
var customContext = new CustomFlowContext();
var workflow = new FFlowBuilder()
    .UseContext(customContext)
    .StartWith<StepOne>()
    .Then<StepTwo>()
    .Build();
```

### Using `UseContext<T>()`
You can also specify a context type using the `UseContext<T>()` method. This will create an instance of the specified context type for your workflow.

```csharp
var workflow = new FFlowBuilder()
    .UseContext<MyCustomFlowContext>() // MyCustomFlowContext implements IFlowContext
    .StartWith<StepOne>()
    .Then<StepTwo>()
    .Build();
```

### Through dependency injection
If you are using dependency injection, you can register your context implementation and then use it in your workflow. FFlow will automatically resolve the context from the service provider. Do keep in mind you still need to use `UseContext<T>()` to specify the type of context you want to use.

```csharp
services.AddScoped<IFlowContext, MyCustomFlowContext>();
var workflow = new FFlowBuilder(services.BuildServiceProvider())
    .UseContext<MyCustomFlowContext>() // Specify the context type
    .StartWith<StepOne>()
    .Then<StepTwo>()
    .Build();
```

> [!NOTE]
> Since the context is returned after executing the workflow, you **have to** call any methods responsible for persisting the context data after the workflow execution, such as `SaveChangesAsync()` or similar methods in your context implementation.

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
